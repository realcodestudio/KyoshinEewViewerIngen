﻿using KyoshinEewViewer.Dmdata.ApiResponses;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;

namespace KyoshinEewViewer.Dmdata.WebSocketMessages
{
	public class DataWebSocketMessage : DmdataWebSocketMessage
	{
		public DataWebSocketMessage()
		{
			Type = "data";
		}

		[JsonPropertyName("classification")]
		public string Classification { get; set; }
		[JsonPropertyName("key")]
		public string Key { get; set; }
		[JsonPropertyName("body")]
		public string Body { get; set; }
		[JsonPropertyName("data")]
		public TelegramData Data { get; set; }
		[JsonPropertyName("xmlData")]
		public TelegramXmldata XmlData { get; set; }

		/// <summary>
		/// bodyのStreamを取得します。
		/// <para>Disposeしてください！</para>
		/// </summary>
		/// <returns></returns>
		public Stream GetBodyStream()
		{
			var memStream = new MemoryStream(Convert.FromBase64String(Body));
			if (!Data.Xml)
				return memStream;
			return new GZipStream(memStream, CompressionMode.Decompress);
		}
		/// <summary>
		/// bodyのStreamを取得します。
		/// <para>Disposeしてください！</para>
		/// </summary>
		/// <returns></returns>
		public string GetBodyString()
		{
			using var stream = GetBodyStream();
			using var memoryStream = new MemoryStream();

			stream.CopyToAsync(memoryStream);

			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}
	}
}