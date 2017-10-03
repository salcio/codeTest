using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Petroineos.CodeTest.Business.Config;
using Petroineos.CodeTest.Business.Reports.Model;

namespace Petroineos.CodeTest.Business.Reports.Writers
{
    public class CsvReportWriter : IReportWriter
    {
        private readonly IConfigStore _contifStore;
        private readonly ILog _logger;

        public CsvReportWriter(IConfigStore contifStore, ILog logger)
        {
            _contifStore = contifStore;
            _logger = logger;
        }

        public async Task WriteAsync(Report report)
        {
            var fileName = $"PowerPosition_{report.GeneratedDate:yyyyMMdd_HHmm}.csv";
            var destinationFolder = _contifStore.ReportFilesDestinationFolder;

            ValidateFolder(destinationFolder);

            var content = CreateReportContent(report);

            await WriteContent(Path.Combine(destinationFolder, fileName), content);
        }

        private async Task WriteContent(string filePath, string content)
        {
            _logger.Debug($"Writing report to file '{filePath}'");
            byte[] encodedText = Encoding.Unicode.GetBytes(content);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }

        private string CreateReportContent(Report report)   
        {
            _logger.Debug("Creating report content");
            var builder = new StringBuilder();
            builder.AppendLine("Local Time,Volume");
            foreach (var reportPoint in report.Points)
            {
                builder.AppendLine($"{reportPoint.LocalTime},{reportPoint.Volume:0.##}");
            }
            _logger.Debug("Finished report content craetion");
            return builder.ToString();
        }

        private void ValidateFolder(string destinationFolder)
        {
            if (Directory.Exists(destinationFolder))
                return;
            _logger.InfoFormat("Destination folder doesn't exists (path: {0}). Creating.", destinationFolder);
            try
            {
                Directory.CreateDirectory(destinationFolder);
            }
            catch (Exception e)
            {
                _logger.ErrorFormat("Cannot create destination folder for reports.", e);
                throw;
            }
        }
    }
}