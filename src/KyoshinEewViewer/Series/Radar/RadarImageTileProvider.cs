﻿using Avalonia.Threading;
using KyoshinEewViewer.Map.Layers.ImageTile;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KyoshinEewViewer.Series.Radar
{
	public class RadarImageTileProvider : ImageTileProvider, IDisposable
	{
		private DateTime BaseTime { get; }
		private DateTime ValidTime { get; }
		public RadarImageTileProvider(DateTime baseTime, DateTime validTime)
		{
			BaseTime = baseTime;
			ValidTime = validTime;
		}

		private bool IsDisposing { get; set; } = false;

		private ConcurrentDictionary<(int z, int x, int y), SKBitmap?> Cache { get; } = new();

		public override int MinZoomLevel { get; } = 4;

		public override int MaxZoomLevel { get; } = 10;

		public override SKBitmap? GetOrStartFetchTileBitmap(int z, int x, int y)
		{
			if (Cache.TryGetValue((z, x, y), out var image))
				return image;
			Task.Run(async () =>
			{
				try
				{
					using var str = await RadarSeries.Client.GetStreamAsync($"https://www.jma.go.jp/bosai/jmatile/data/nowc/{BaseTime:yyyyMMddHHmm00}/none/{ValidTime:yyyyMMddHHmm00}/surf/hrpns/{z}/{x}/{y}.png");
					if (IsDisposing)
						return;
					var bitmap = SKBitmap.Decode(str);
					unsafe
					{
						var ptr = (uint*)bitmap.GetPixels().ToPointer();
						var pixelCount = bitmap.Width * bitmap.Height;

						for (var i = 0; i < pixelCount; i++)
							*ptr++ &= 0xAE_FF_FF_FF;
					}
					Cache[(z, x, y)] = bitmap;
					if (bitmap != null)
						OnImageFetched();
				}
				catch(Exception ex)
				{
					Debug.WriteLine(ex);
					Cache[(z, x, y)] = null;
				}
			});
			return null;
		}

		public void Dispose()
		{
			IsDisposing = true;
			foreach (var b in Cache.Values)
				b?.Dispose();
			Cache.Clear();
			GC.SuppressFinalize(this);
		}
	}
}