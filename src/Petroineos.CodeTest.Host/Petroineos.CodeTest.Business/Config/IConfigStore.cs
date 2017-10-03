namespace Petroineos.CodeTest.Business.Config
{
    public interface IConfigStore
    {
        int ReportIntervalInMinutes { get; }
        int MaxRetriesOnServiceError { get; }
        int DelayBetweenRetiesInMiliseconds { get; }
        string ReportFilesDestinationFolder { get; }
    }
}