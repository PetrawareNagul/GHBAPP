using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility.Helper
{
    public static class CustomGenerator
    {
        public static string GenerateSixDigitPin()
        {
            Random generator = new Random();
            return generator.Next(0, 1000000).ToString().PadLeft(6, '0');
        }
        public static string GenerateUniqueKeyForUser(string username)
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string key = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');
            //string key = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            return key;
        }
        public static string GenerateGoogleAuthenticationUniqueKeyForEachUser(string username)
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string key = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');
            //string key = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            return key;
        }

        public static List<int> SplitNumber(int minAmount, int maxAmount, int maxPerGroup)
        {
            List<int> result = new List<int>();

            int minNo = minAmount;
            int interval = (maxAmount - minAmount) / maxPerGroup;
            for (int i = 0; i < maxPerGroup; i++)
            {
                result.Add(minNo);
                minNo = minNo + interval;
            }
            result.Add(maxAmount);
            return result;
        }

        public static string StageByStageId(BatchesStages batchesStages)
        {
            switch (batchesStages)
            {
                case BatchesStages.Scan:
                    {
                        return BatchesStages.Scan.ToString();
                    }
                case BatchesStages.Index:
                    {
                        return BatchesStages.Index.ToString();
                    }
                case BatchesStages.Export:
                    {
                        return BatchesStages.Export.ToString();
                    }
                case BatchesStages.Integrate:
                    {
                        return BatchesStages.Integrate.ToString();
                    }
                case BatchesStages.Release:
                    {
                        return BatchesStages.Release.ToString();
                    }
                case BatchesStages.Document:
                    {
                        return BatchesStages.Document.ToString();
                    }
                default: break;

            }
            return "";
        }


    }
    public class BatchesStagesCount
    {
        public int StageId { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public int count { get; set; }
        public int SetCount { get; set; }
    }

    public class StatisticModel
    {
        public int StageId { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public int count { get; set; }
        public int SetCount { get; set; }
        public string DepartmentCode { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class BatchesInfo
    {
        public List<Batch> Batches { get; set; }
        public List<BatchLog> BatcheLogs { get; set; }
    }
    public enum BatchesStages
    {
        Scan = 1,
        Index = 2,
        Export = 3,
        Integrate = 4,
        Release = 5,
        Document = 6
    };
}
