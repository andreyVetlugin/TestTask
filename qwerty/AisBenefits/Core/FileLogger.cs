using System;
using System.IO;

namespace AisBenefits.Core
{
    public class FileLogger: IDisposable
    {
        private readonly StreamWriter file;
        private readonly string directory;
        private readonly string context;

        public FileLogger(string directory, string context, DateTime dateTime)
            : this(directory, $"{dateTime:hh_mm_ss_fff}_{context}")
        {
        }

        public FileLogger(string directory, string context)
        {
            this.directory = directory;
            this.context = context;
            Directory.CreateDirectory(directory);

            file = new StreamWriter(new FileStream(Path.Combine(directory, $"{context}.log"), FileMode.Append));
        }

        public void Log(string content)
        {
            file.WriteLine(content);
        }

        public static void Log(string directory, string context, DateTime dateTime, string content)
        {
            using (var logger = new FileLogger(directory, context, dateTime))
            {
                logger.Log(content);
            }
        }

        public static void Log(string directory, string context, string content)
        {
            using (var logger = new FileLogger(directory, context))
            {
                logger.Log(content);
            }
        }

        public void Dispose()
        {
            file.Dispose();
        }
    }
}
