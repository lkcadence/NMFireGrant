using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace NMSFM.Services.CPSystem
{
	internal class TripleDES : IDisposable
	{
		// define the triple des provider
		private TripleDESCryptoServiceProvider m_des = new TripleDESCryptoServiceProvider();
		private UTF8Encoding m_utf8 = new UTF8Encoding();

		// define the local property arrays
		private byte[] m_key = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };
		private byte[] m_iv = { 1, 0, 2, 4, 1, 9, 7, 5 };
		// below are valid formats for initialization vector and key. iv is 8 random bytes and key is 24 random values
		// Private ReadOnly key() As Byte = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24}
		// Private ReadOnly iv() As Byte = {1, 0, 2, 4, 1, 9, 7, 5}

		void IDisposable.Dispose()
		{
			// Throw New NotImplementedException()
			m_des.Dispose();
			m_utf8 = null;
			m_key = null;
			m_iv = null;
		}

		public TripleDES()
		{
		}

		/// <summary>
		///     ''' This routine creates a new encrypted key.
		///     ''' </summary>
		///     ''' <param name="key"></param>
		///     ''' <param name="iv"></param>
		///     ''' <remarks></remarks>
		//public TripleDES(byte[] key, byte[] iv)
		//{
		//	try
		//	{
		//		m_key = key;
		//		m_iv = iv;
		//	}
		//	catch (Exception ex)
		//	{
		//		HandleError(ex, "TripleDES", "New - Constructor");
		//	}
		//	finally
		//	{
		//	}
		//}

		/// <summary>
		///     ''' This routine sets the encryption key.
		///     ''' </summary>
		///     ''' <param name="key"></param>
		///     ''' <param name="iv"></param>
		///     ''' <remarks></remarks>
		//protected void SetKeys(byte[] key, byte[] iv)
		//{
		//	// this is used if the constructor is called without
		//	// parameters for the iv and key
		//	try
		//	{
		//		m_key = key;
		//		m_iv = iv;
		//	}
		//	catch (Exception ex)
		//	{
		//		HandleError(ex, "TripleDES", "SetKeys");
		//	}
		//	finally
		//	{
		//	}
		//}

		/// <summary>
		///     ''' This function encrypts the bytes of data.
		///     ''' </summary>
		///     ''' <param name="input">    The input data to encrypt.</param>
		///     ''' <returns>               The encrypted data.</returns>
		///     ''' <remarks></remarks>
		public byte[] Encrypt(byte[] input)
		{
			byte[] result = null;
			try
			{
				result = Transform(input, m_des.CreateEncryptor(m_key, m_iv));
			}
			catch (Exception)
			{
				//HandleError(ex, "TripleDES", "Encrypt(Byte)");
			}
			return result;
		}

		/// <summary>
		///     ''' This function decrypts the bytes of data.
		///     ''' </summary>
		///     ''' <param name="input">    The input data to decrypt.</param>
		///     ''' <returns>               The decrypted data.</returns>
		///     ''' <remarks></remarks>
		public byte[] Decrypt(byte[] input)
		{
			byte[] result = null;
			try
			{
				result = Transform(input, m_des.CreateDecryptor(m_key, m_iv));
			}
			catch (Exception)
			{
				//HandleError(ex, "TripleDES", "Decrypt(Byte)");
			}
			return result;
		}

		/// <summary>
		///     ''' This function encrypts a string of data.
		///     ''' </summary>
		///     ''' <param name="text">     The text.</param>
		///     ''' <returns>               The encrypted string.</returns>
		///     ''' <remarks></remarks>
		public string Encrypt(string text)
		{
			string result = "";
			try
			{
				byte[] input = m_utf8.GetBytes(text);
				byte[] output = Transform(input, m_des.CreateEncryptor(m_key, m_iv));
				result = Convert.ToBase64String(output);
			}
			catch (Exception)
			{
				//HandleError(ex, "TripleDES", "Encrypt(Text)");
			}
			return result;
		}

		/// <summary>
		///     ''' This function decrypts a string of data.
		///     ''' </summary>
		///     ''' <param name="text">    The text.</param>
		///     ''' <returns>              The decrypted string.</returns>
		///     ''' <remarks></remarks>
		public string Decrypt(string text)
		{
			string result = "";
			try
			{
				byte[] input = Convert.FromBase64String(text);
				byte[] output = Transform(input, m_des.CreateDecryptor(m_key, m_iv));
				result = m_utf8.GetString(output);
			}
			catch (Exception)
			{
				// HandleError(ex, "TripleDES", "Decrypt(Text)")
				//throw ex;
				//logger.Error("Unexpected exception caught while decrypting the value '" + text + "'.", ex);
			}
			return result;
		}

		/// <summary>
		///     ''' This functionconverts the raw bytes or string to encrypted or decrypted data.
		///     ''' </summary>
		///     ''' <param name="input">              The input.</param>
		///     ''' <param name="CryptoTransform">    The CryptoTransform which handles the process of encrypting or decrypting data.</param>
		///     ''' <returns>                         Returns the encrypted or decrypted data.</returns>
		///     ''' <remarks></remarks>
		private byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
		{
			byte[] result = null;
			try
			{
				// create the necessary streams
				MemoryStream memStream = new MemoryStream();
				CryptoStream cryptStream = new CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write);

				// transform the bytes as requested
				cryptStream.Write(input, 0, input.Length);
				cryptStream.FlushFinalBlock();

				// Read the memory stream and convert it back into byte array
				memStream.Position = 0;
				result = memStream.ToArray();

				// close and release the streams
				memStream.Close();
				cryptStream.Close();

				// return the encrypted buffer
				//Transform = result;
			}
			catch (Exception)
			{
				//HandleError(ex, "TripleDES", "Transform");
			}
			return result;
		}
	}
}
