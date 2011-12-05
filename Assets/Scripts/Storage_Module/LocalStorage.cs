using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;

public class LocalStorage {
	public const int BUF_SIZE = 64;
	private static UTF8Encoding enc = new UTF8Encoding();
	/*
	 * Writes (and creates if necessary) a file in local storage
	 * and fills it with the content of a byte array.
	 */
	public static void WriteBinaryFile(string name, byte[] data) {
		var w = new BinaryWriter(File.Open(getLocalStoragePath()+"/"+name, FileMode.Create));
		w.Write(data);
		w.Close();
	}

	/*
	 * Reads a whole file from local storage into an byte array.
	 *
	 * The file name is appended to the local storage path and opened
	 * read-only.
	 * The file is read in BUF_SIZE chunks (64 bytes by default).
	 */
	public static byte[] ReadBinaryFile(string name) {
		var r = new BinaryReader(File.Open(getLocalStoragePath()+"/"+name, FileMode.Open));
		byte[] result = new byte[0];
		byte[] buf = new byte[BUF_SIZE];
		int read, count = 0;;
		do {
			//read = r.Read(buf, count*BUF_SIZE, BUF_SIZE);
			read = r.Read(buf, 0, BUF_SIZE);
			byte[] new_result = new byte[count*BUF_SIZE + read];
			Array.Copy(result, 0, new_result, 0, count*BUF_SIZE);
			Array.Copy(buf, 0, new_result, count*BUF_SIZE, read);
			result = new_result;
			count++;
		} while (read == BUF_SIZE);
		r.Close();
		return result;
	}

	/*
	 * Reads a whole UTF8 formatted file from local storage into a string.
	 */
	public static string ReadUTF8File(string name) {
		var data = ReadBinaryFile(name);
		return enc.GetString(data);
	}

	/**
	 * Writes (and creates if necessary) an UTF8 formatted file in local
	 * storage and fills it with the content of the string.
	 */
	public static void WriteUTF8File(string name, string data) {
		WriteBinaryFile(name, enc.GetBytes(data));
	}

	private static string getLocalStoragePath()  {
		return Application.persistentDataPath;
	}
}
