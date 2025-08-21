using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Aftershock.Components
{
	// Token: 0x020000BF RID: 191
	public static class _Gzip
	{
		// Token: 0x06000738 RID: 1848 RVA: 0x000163B8 File Offset: 0x000145B8
		public static string Decompress(byte[] gzip)
		{
			string result;
			using (GZipStream gzipStream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
			{
				byte[] buffer = new byte[4096];
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int num;
					do
					{
						num = gzipStream.Read(buffer, 0, 4096);
						if (num > 0)
						{
							memoryStream.Write(buffer, 0, num);
						}
					}
					while (num > 0);
					memoryStream.Position = 0L;
					StreamReader streamReader = new StreamReader(memoryStream);
					string text = streamReader.ReadToEnd();
					result = text;
				}
			}
			return result;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00016458 File Offset: 0x00014658
		public static byte[] Compress(string text)
		{
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			byte[] bytes = utf8Encoding.GetBytes(text);
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
				{
					gzipStream.Write(bytes, 0, bytes.Length);
				}
				memoryStream.Position = 0L;
				StreamReader streamReader = new StreamReader(memoryStream);
				streamReader.ReadToEnd();
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x000164E4 File Offset: 0x000146E4
		public static string Write(string value)
		{
			byte[] array = new byte[Encoding.UTF8.GetBytes(value).Length];
			int num = 0;
			foreach (char c in Encoding.UTF8.GetBytes(value))
			{
				array[num++] = (byte)c;
			}
			MemoryStream memoryStream = new MemoryStream();
			GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress);
			gzipStream.Write(array, 0, array.Length);
			gzipStream.Close();
			array = memoryStream.ToArray();
			StringBuilder stringBuilder = new StringBuilder(array.Length);
			foreach (byte value2 in array)
			{
				stringBuilder.Append((char)value2);
			}
			memoryStream.Close();
			gzipStream.Dispose();
			memoryStream.Dispose();
			return stringBuilder.ToString();
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000165AC File Offset: 0x000147AC
		public static string Read(string sData)
		{
			byte[] array = Encoding.UTF8.GetBytes(sData);
			int num = 0;
			foreach (char c in Encoding.UTF8.GetBytes(sData))
			{
				array[num++] = (byte)c;
			}
			MemoryStream memoryStream = new MemoryStream(array);
			GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
			array = new byte[1024];
			StringBuilder stringBuilder = new StringBuilder();
			int num2;
			while ((num2 = gzipStream.Read(array, 0, array.Length)) != 0)
			{
				for (int j = 0; j < num2; j++)
				{
					stringBuilder.Append((char)array[j]);
				}
			}
			gzipStream.Close();
			memoryStream.Close();
			gzipStream.Dispose();
			memoryStream.Dispose();
			return stringBuilder.ToString();
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001666C File Offset: 0x0001486C
		public static XDocument UnZipXML(string XMLString)
		{
			XDocument result;
			try
			{
				XDocument xdocument = XDocument.Parse(XMLString);
				result = xdocument;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001669C File Offset: 0x0001489C
		public static byte[] ZipXML(XDocument textReader)
		{
			if (textReader == null)
			{
				Debug.Log("ZipXML: textReader was null");
				return null;
			}
			string text = textReader.ToString();
			return _Gzip.Compress(text);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000166C8 File Offset: 0x000148C8
		public static string ZipXML(string FullPath)
		{
			TextReader textReader = new StreamReader(FullPath);
			string value = textReader.ReadToEnd();
			textReader.Close();
			return _Gzip.Write(value);
		}
	}
}