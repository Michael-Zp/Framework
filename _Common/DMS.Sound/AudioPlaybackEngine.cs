using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.IO;

namespace DMS.Sound
{
	public class AudioPlaybackEngine : IDisposable
	{
		private readonly IWavePlayer outputDevice;
		private readonly MixingSampleProvider mixer;

		public AudioPlaybackEngine(int sampleRate = 44100, int channelCount = 2)
		{
			outputDevice = new WaveOutEvent();
			mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
			mixer.ReadFully = true;
			outputDevice.Init(mixer);
			outputDevice.Play();
		}

		private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
		{
			if (input.WaveFormat.Channels == mixer.WaveFormat.Channels)
			{
				return input;
			}
			if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2)
			{
				return new MonoToStereoSampleProvider(input);
			}
			throw new NotImplementedException("Not yet implemented this channel count conversion");
		}

		public void PlaySound(string fileName, bool looped = false)
		{
			var input = new AudioFileReader(fileName);
			if (looped)
			{
				var reader = new SoundLoopStream(input);
				var sampleChannel = new SampleChannel(reader, false);
				AddMixerInput(sampleChannel);
			}
			else
			{
				AddMixerInput(new AutoDisposeSampleProvider(input, input));
			}
		}

		/// <summary>
		/// Plays sound from a stream; you get unbuffered access if you usse a file stream 
		/// and buffered access if you use a memory stream
		/// </summary>
		/// <param name="stream"></param>
		public void PlaySound(Stream stream, bool looped = false)
		{
			WaveStream reader = new WaveFileReader(stream);
			if (looped)
			{
				reader = new SoundLoopStream(reader);
				var sampleChannel = new SampleChannel(reader, false);
				AddMixerInput(sampleChannel);
			}
			else
			{
				var sampleChannel = new SampleChannel(reader, false);
				AddMixerInput(new AutoDisposeSampleProvider(sampleChannel, reader));
			}
		}

		private void AddMixerInput(ISampleProvider input)
		{
			mixer.AddMixerInput(ConvertToRightChannelCount(input));
		}

		public void Dispose()
		{
			outputDevice.Dispose();
		}
	}
}