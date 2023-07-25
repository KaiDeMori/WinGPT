﻿using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace WinGPT;

public static class Tools {
   public readonly struct ErrorOr<SUCCESS_TYPE, ERROR_TYPE> {
      private readonly SUCCESS_TYPE result;
      private readonly ERROR_TYPE?  error;

      private ErrorOr(SUCCESS_TYPE result, ERROR_TYPE? error) {
         this.result = result;
         this.error  = error;
      }

      public bool IsError => error != null;

      public SUCCESS_TYPE Result {
         get {
            if (IsError) {
               throw new InvalidOperationException("Cannot get result of an error");
            }

            return result;
         }
      }

      public ERROR_TYPE Error {
         get {
            if (!IsError) {
               throw new InvalidOperationException("Cannot get error of a successful result");
            }

            return error!;
         }
      }

      public static implicit operator ErrorOr<SUCCESS_TYPE, ERROR_TYPE>(SUCCESS_TYPE result) {
         return new ErrorOr<SUCCESS_TYPE, ERROR_TYPE>(result, default); // No compiler warning
      }

      public static implicit operator ErrorOr<SUCCESS_TYPE, ERROR_TYPE>(ERROR_TYPE error) {
         return new ErrorOr<SUCCESS_TYPE, ERROR_TYPE>(default!, error); // Use default! to satisfy non-nullable 'T' 
      }

      public static implicit operator SUCCESS_TYPE(ErrorOr<SUCCESS_TYPE, ERROR_TYPE> result) {
         return result.Result;
      }

      public static implicit operator ERROR_TYPE(ErrorOr<SUCCESS_TYPE, ERROR_TYPE> result) {
         return result.Error;
      }
   }

   /// <summary>
   /// Checks if the given object has any null properties.
   /// Ignores static properties.
   /// </summary>
   /// <param name="obj">The object to check for null properties.</param>
   /// <returns>
   /// <c>true</c> if the given object has any null properties; otherwise, <c>false</c>.
   /// </returns>
   public static bool HasNullProperties([NotNullWhen(returnValue: false)] object? obj) {
      if (obj == null)
         return true;
      var type = obj.GetType();
      if (type.IsPrimitive)
         return false;
      if (type == typeof(string))
         return string.IsNullOrEmpty((string) obj);

      // Check if the object is a dictionary
      if (typeof(IDictionary).IsAssignableFrom(type)) {
         var dictionary = (IDictionary) obj;
         return dictionary.Keys.Cast<object?>().Any(HasNullProperties) ||
                dictionary.Values.Cast<object?>().Any(HasNullProperties);
      }

      if (!type.IsArray)
         return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => !property.GetIndexParameters().Any()) // Filter out indexer properties
            .Any(property => HasNullProperties(property.GetValue(obj)));

      var array = (Array) obj;
      return array.Cast<object?>().Any(HasNullProperties);
   }

   public static string To_valid_filename(string filename) {
      var invalidChars = Path.GetInvalidFileNameChars();

      if (filename.IndexOfAny(invalidChars) >= 0) {
         filename = invalidChars
            .Aggregate(filename, (current, invalidChar) =>
               current.Replace(invalidChar, '_'));
      }

      filename = filename.Trim();

      return filename;
   }

   public static string GetRelativePath(FileInfo file, DirectoryInfo baseDir) {
      // Check if the FileInfo object is null and throw an exception if it is
      if (file == null)
         throw new ArgumentNullException(nameof(file));

      // Check if the DirectoryInfo object is null and throw an exception if it is
      if (baseDir == null)
         throw new ArgumentNullException(nameof(baseDir));

      // Retrieve the full path of the file
      string filePath = file.FullName;

      // Retrieve the full path of the base directory
      string baseDirPath = baseDir.FullName;

      // Check if the file's full path starts with the base directory's full path
      if (filePath.StartsWith(baseDirPath)) {
         // If the file path starts with the base directory path, get the relative path
         // by removing the base directory path from the start of the file path.
         // Also remove any leading directory separator that might remain after the removal.
         return filePath.Substring(baseDirPath.Length).TrimStart(Path.DirectorySeparatorChar);
      }
      else {
         // If the file path doesn't start with the base directory path, it's not possible to get a relative path.
         // So, we throw an exception.
         throw new ArgumentException("File path does not start with base directory path.");
      }
   }
}