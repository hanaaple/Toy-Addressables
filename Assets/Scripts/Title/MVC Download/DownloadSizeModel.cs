namespace Title.MVC_Download
{
    public struct DownloadSizeModel
    {
        private static readonly string[] Units = { "B", "KB", "MB", "GB" };

        public long DownloadSize;

        public (long, string) GetConvertedDownloadSize() {
            long temp = DownloadSize;
            int index = 0;
            while (temp >= 1024 && Units.Length - 1 > index)
            {
                temp /= 1024;
                index++;
            }

            return (temp, Units[index]);
        }

        public DownloadSizeModel(long downloadSize)
        {
            DownloadSize = downloadSize;
        }
    }
}