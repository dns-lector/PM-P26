using App.Services.Helper;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services.Helper
{
    [TestClass]
    public sealed class HelperTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var helper = new App.Services.Helper.Helper();
        }

        [TestMethod]
        public void PathCombineTest()
        {
            var helper = new App.Services.Helper.Helper();
            Assert.AreEqual(
                "/dir1/file.ext",
                helper.PathCombine("/dir1", "file.ext"),
                "Combination rule: '/dir1'  + 'file.ext' -> '/dir1/file.ext'"
            );
            Assert.AreEqual(
                "/dir2/file2.ext",
                helper.PathCombine("/dir2", "/file2.ext"),
                "Combination rule: '/dir2'  + '/file2.ext' -> '/dir2/file2.ext'"
            );
            Assert.AreEqual(
                "/subdir3/file3.ext",
                helper.PathCombine("/subdir3/", "//file3.ext"),
                "Combination rule: '/subdir3/'  + '//file3.ext' -> '/subdir3/file3.ext'"
            );
            Assert.AreEqual(
                "/dir/subdir/file.ext",
                helper.PathCombine("dir", "subdir", "file.ext"),
                "Combination rule: 'dir' + 'subdir'  + 'file.ext' -> '/dir/subdir/file.ext'"
            );
            Assert.AreEqual(
                "/dir/subdir1/subdir2/subdir3/file.ext",
                helper.PathCombine("/dir/", "/subdir1/", "/subdir2/", "/subdir3/", "/file.ext"),
                "Combination rule: '/dir/' + '/subdir1/' + '/subdir2/' + '/subdir3/'  + '/file.ext' -> '/dir/subdir/file.ext'"
            );

            Assert.AreEqual(
                "/dir2/file2.ext",
                helper.PathCombine(@"\dir2", @"\file2.ext"),
                @"Combination rule: '\dir2'  + '\file2.ext' -> '/dir2/file2.ext'"
            );
            Assert.AreEqual(
                @"C://dir/file",
                helper.PathCombine(@"C://dir/file"),
                @"Combination rule: 'C://dir/file' -> 'C://dir/file'"
            );
            Assert.AreEqual(
                @"/file",
                helper.PathCombine(@"file"),
                @"Combination rule: 'file' -> '/file'"
            );
            Assert.AreEqual(
                @"C://dir/file",
                helper.PathCombine(@"C://dir", "file"),
                @"Combination rule: 'C://dir' + 'file' -> 'C://dir/file'"
            );

            Assert.AreEqual(
                @"/file",
                helper.PathCombine(@"dir", "..", "file"),
                @"Combination rule: 'dir' + '..' + 'file' -> '/file'"
            );
            Assert.AreEqual(
                @"/dir2/file",
                helper.PathCombine(@"dir", "../dir2", "file"),
                @"Combination rule: 'dir' + '../dir2' + 'file' -> '/dir2/file'"
            );
            Assert.AreEqual(
                @"/dir/dir2/file",
                helper.PathCombine(@"dir/dir1", "../dir2", "file"),
                @"Combination rule: 'dir/dir1' + '../dir2' + 'file' -> '/dir/dir2/file'"
            );

            Assert.AreEqual(
                @"C://file",
                helper.PathCombine(@"C://dir", "..", "file"),
                @"Combination rule: 'C://dir' + '..' + 'file' -> 'C://file'"
            );

            Assert.AreEqual(
                @"ftp://file",
                helper.PathCombine(@"ftp://dir", "..", "file"),
                @"Combination rule: 'ftp://dir' + '..' + 'file' -> 'ftp://file'"
            );

            Assert.AreEqual(
                @"http://dir/sub/file",
                helper.PathCombine(@"http://dir/", "./sub", "./file"),
                @"Combination rule: 'http://dir/' + './sub' + './file' -> 'http://dir/sub/file'"
            );

            Assert.AreEqual(
                @"http://dir/sub/file.txt",
                helper.PathCombine(@"http://dir/", " ./sub", "  ./file.txt"),
                @"Combination rule: 'http://dir/' + ' ./sub' + '  ./file.txt' -> 'http://dir/sub/file.txt'"
            );

            Assert.AreEqual(
                @"http://dir/sub/file.txt",
                helper.PathCombine(@" http://dir/ ", " ./sub\t", "  ./file.txt\r\n"),
                @"Combination rule: ' http://dir/ ' + ' ./sub\t' + '  ./file.txt\r\n' -> 'http://dir/sub/file.txt'"
            );

            // ThrowsException порівнює типи суворо, спадкування НЕ враховується
            var ex = Assert.ThrowsException<ArgumentException>(
                () => helper.PathCombine(),
                "helper.PathCombine() without argument must throw exception"
            );
            Assert.IsTrue(
                ex.Message.Contains("'parts'"),
                "helper.PathCombine() exception message must contain 'parts' word"
            );
            Assert.IsTrue(
                ex.Message.Contains("must not be empty"),
                "helper.PathCombine() exception message must contain 'must not be empty' phrase"
            );

            Assert.AreEqual(
                "Invalid symbol '*' in argument 0 ('dir*')",
                Assert.ThrowsException<ArgumentException>(
                    () => helper.PathCombine("dir*"),
                    "helper.PathCombine('dir*') must throw ArgumentException"
                ).Message,
                "helper.PathCombine('dir*') ex.Message must be 'Invalid symbol '*' in argument 0 ('dir*')'"
            );
            foreach (String path in new String[] { "dir*2", "sub*", "sub2*", "file*.txt", "*subdir" })
            {
                Assert.AreEqual(
                    $"Invalid symbol '*' in argument 1 ('{path}')",
                    Assert.ThrowsException<ArgumentException>(
                        () => helper.PathCombine("dir", path),
                        $"helper.PathCombine('dir', '{path}') must throw ArgumentException"
                    ).Message,
                    $"helper.PathCombine('dir', '{path}') ex.Message must be 'Invalid symbol '*' in argument 1 ('{path}')'"
                );
            }

            {
                String path = "file?.txt";
                String msg = $"Invalid symbol '?' in argument 1 ('{path}')";
                Assert.AreEqual(
                    msg,
                    Assert.ThrowsException<ArgumentException>(
                        () => helper.PathCombine("dir", path, "obj"),
                        $"helper.PathCombine('dir', '{path}', 'obj') must throw ArgumentException"
                    ).Message,
                    $"helper.PathCombine('dir', '{path}', 'obj') ex.Message must be '{msg}'"
                );
            }

            {
                String path = "sub?";
                String msg = $"Invalid symbol '?' in argument 2 ('{path}')";
                Assert.AreEqual(
                    msg,
                    Assert.ThrowsException<ArgumentException>(
                        () => helper.PathCombine("dir", "obj", path, "file"),
                        $"helper.PathCombine('dir', 'obj', '{path}', 'file') must throw ArgumentException"
                    ).Message,
                    $"helper.PathCombine('dir', 'obj', '{path}', 'file') ex.Message must be '{msg}'"
                );
            }

            Dictionary<String[], int> testCases = new()
            {
                { ["dir", "obj", "sub?", "file"], 2 },
                { ["dir*", "obj", "sub?", "file"], 0 },
                { ["dir", "obj*", "sub?", "file"], 1 },
                { ["dir", "obj", "sub", "file?"], 3 },
                { ["?obj", "sub", "file?"], 0 },
                { ["obj", "?sub", "file?"], 1 },
                { ["obj", "sub", "file*"], 2 },
                { ["ob*j", "sub"], 0 },
                { ["sub", "file*"], 1 },
            };
            foreach( var testCase in testCases)
            {
                char c = testCase.Key[testCase.Value].Contains('*') ? '*' : '?';
                String msg = $"Invalid symbol '{c}' in argument {testCase.Value} " +
                    $"('{testCase.Key[testCase.Value]}')";
                String invocation = $"helper.PathCombine({String.Join(", ", testCase.Key)})";
                Assert.AreEqual(
                    msg,
                    Assert.ThrowsException<ArgumentException>(
                        () => helper.PathCombine(testCase.Key),
                        $"{invocation} must throw ArgumentException"
                    ).Message,
                    $"{invocation} ex.Message must be '{msg}'"
                );
            }

        }

        [TestMethod]
        public void PathCombineInvalidRootPositionTest()
        {
            var helper = new App.Services.Helper.Helper();
            Dictionary<String[], int> testCases = new()
            {
                { ["dir", "sub", "C://"], 2 },
                { ["dir", "D://", "sub"], 1 },
                { ["dir", "E://", "sub", "sub2"], 1 },
                { ["dir", "sub", "ftp://", "sub2"], 2 },
                { ["dir", "sub", "sub2", "http://"], 3 },
                { ["dir", "ws://"], 1 },
            };
            foreach (var testCase in testCases)
            {
                String msg = $"Invalid sequence: root path '{testCase.Key[testCase.Value]}' " +
                    $"could not be at position {testCase.Value}";
                String invocation = $"helper.PathCombine({String.Join(", ", testCase.Key)})";
                Assert.AreEqual(
                    msg,
                    Assert.ThrowsException<ArgumentException>(
                        () => helper.PathCombine(testCase.Key),
                        $"{invocation} must throw ArgumentException"
                    ).Message,
                    $"{invocation} ex.Message must be '{msg}'"
                );
            }
        }
    }
}
