using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.Services.Helper
{
    public class Helper
    {
        // public String PathCombine(String dir, String file) => "/dir1/file.ext";
        // public String PathCombine(params String[] parts)
        // {
        //     char[] chars = ['/', '\\'];
        //     return String.Join("",
        //         parts.Select(part => part.Contains("://")
        //             ? part
        //             : $"/{part.TrimEnd(chars).TrimStart(chars)}"));            
        // }
        public String PathCombine(params String[] parts)
        {
            if(parts.Length == 0)
            {
                throw new ArgumentException("'parts' must not be empty");
            }
            char[] chars = ['/', '\\', ' '];
            char[] restrictedChars = ['*', '?'];
            LinkedList<String> list = [];
            for (int i = 0; i < parts.Length; ++i )
            {
                String path = parts[i];
                foreach (char c in restrictedChars)
                {
                    if (path.Contains(c))
                    {
                        throw new ArgumentException(
                            $"Invalid symbol '{c}' in argument {i} ('{path}')"
                        );
                    }
                }
                foreach (String fragment in Regex.Split(path, @"(?<!/)/(?!/)"))
                {
                    String part = fragment.Trim();
                    if (part == "" || part == ".") continue;
                    if (part == "..") list.RemoveLast();
                    else
                    {
                        int index = part.IndexOf("://");
                        if (index != -1)                  // C://dir
                        {
                            // Перевіряємо чи це перший аргумент (кореневий елемент
                            // не може бути в іншій позиції)
                            if(i != 0)
                            {
                                throw new ArgumentException("Invalid sequence: root path 'C://' could not be at position 2");
                            }
                            index += 3;
                            String disk = part[..index];
                            list.AddLast(disk);
                            if (part.Length > index)
                            {
                                list.AddLast(part[index..]);
                            }                            
                        }
                        else
                        {
                            list.AddLast($"/{part.TrimEnd(chars).TrimStart(chars)}");
                        }
                    }
                }
            }
            return String.Join("", list).Replace("///", "//");
        }
    }
}
/* Д.З. Продовжити впровадження модульних тестів до 
 * курсового проєкту. Реалізувати тестування виняткових ситуацій.
 */
/*
 1 2 3 4 5
 ""       | 
 "1"      | garbage
 "12"     | N * N/2  -- O(N^2)
 "123"    | 
 "1234"   | 
 "12345"
 */
// Comment GH
// Comment from VS
