using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
#if WINDOWS
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
#endif

namespace GBSPlayer
{
    public class CDynamicEffectInstance
    {
#if WINDOWS
        struct AudioBlock
        {
            public AudioBuffer AudioBuffer;
            public byte[] ByteBuffer;
        }

        private object _voiceLock = new Object();
        private static ByteBufferPool _bufferPool = new ByteBufferPool();
        private Queue<AudioBlock> _queuedBlocks = new Queue<AudioBlock>();

        private SourceVoice _voice;
        private WaveFormat _format;
#else
        private DynamicSoundEffectInstance _instance;
#endif

        public SoundState State = SoundState.Stopped;

        public CDynamicEffectInstance(int sampleRate)
        {
#if WINDOWS
            var xaudio2 = new XAudio2();
            var masteringVoice = new MasteringVoice(xaudio2);

            _format = new WaveFormat(sampleRate, 1);
            _voice = new SourceVoice(xaudio2, _format, true);
            _voice.BufferEnd += OnBufferEnd;
#else
            _instance = new DynamicSoundEffectInstance(sampleRate, AudioChannels.Mono);
#endif
        }

        public int GetPendingBufferCount()
        {
#if WINDOWS
            lock (_voiceLock)
            {
                return _queuedBlocks.Count;
            }
#else
            return _instance.PendingBufferCount;
#endif
        }

        public void Play()
        {
            State = SoundState.Playing;
#if WINDOWS
            lock (_voiceLock)
            {
                _voice.Start();
            }
#else
            _instance.Play();
#endif
        }

        public void Pause()
        {
            State = SoundState.Paused;
#if WINDOWS
            lock (_voiceLock)
            {
                _voice.Stop();
            }
#else
            _instance.Pause();
#endif
        }

        public void Resume()
        {
            State = SoundState.Playing;
#if WINDOWS
            lock (_voiceLock)
            {
                _voice.Start();
            }
#else
            _instance.Resume();
#endif
        }

        public void Stop()
        {
            State = SoundState.Stopped;
#if WINDOWS
            lock (_voiceLock)
            {
                _voice.Stop();
                // Dequeue all the submitted buffers
                _voice.FlushSourceBuffers();
            }
#else
            _instance.Stop();
#endif
        }

        public void SetVolume(float volume)
        {
#if WINDOWS
            lock (_voiceLock)
            {
                _voice.SetVolume(volume);
            }
#else
            _instance.Volume = volume;
#endif
        }

        public void SubmitBuffer(byte[] buffer, int offset, int count)
        {
#if WINDOWS
            var audioBlock = new AudioBlock();

            audioBlock.ByteBuffer = _bufferPool.Get(count);

            // we need to copy so datastream does not pin the buffer that the user might modify later
            Buffer.BlockCopy(buffer, offset, audioBlock.ByteBuffer, 0, count);

            var stream = DataStream.Create(audioBlock.ByteBuffer, true, false, 0, true);
            audioBlock.AudioBuffer = new AudioBuffer(stream);
            audioBlock.AudioBuffer.AudioBytes = count;

            _queuedBlocks.Enqueue(audioBlock);

            lock (_voiceLock)
                _voice.SubmitSourceBuffer(audioBlock.AudioBuffer, null);
#else
            _instance.SubmitBuffer(buffer, offset, count);
#endif
        }

#if WINDOWS
        private void OnBufferEnd(IntPtr obj)
        {
            // Release the buffer
            if (_queuedBlocks.Count > 0)
            {
                var block = _queuedBlocks.Dequeue();
                block.AudioBuffer.Stream.Dispose();
                _bufferPool.Return(block.ByteBuffer);
            }
        }
#endif
    }
}
