using System;
using System.Collections;

namespace SanicballCore
{
    public static class Utils
    {
        public static string GetTimeString(TimeSpan timeToUse)
        {
            return string.Format("{0:00}:{1:00}.{2:000}", timeToUse.Minutes, timeToUse.Seconds, timeToUse.Milliseconds);
        }

        public static string GetPosString(int pos)
        {
            if (pos % 10 == 1 && pos % 100 != 11) return pos + "st";
            if (pos % 10 == 2 && pos % 100 != 12) return pos + "nd";
            if (pos % 10 == 3 && pos % 100 != 13) return pos + "rd";
            return pos + "th";
        }

        public static void Write(this Lidgren.Network.NetBuffer target, Guid guid)
        {
            byte[] guidBytes = guid.ToByteArray();
            target.Write(guidBytes.Length);
            target.Write(guidBytes);
        }

        public static Guid ReadGuid(this Lidgren.Network.NetBuffer target)
        {
            int guidLength = target.ReadInt32();
            byte[] guidBytes = target.ReadBytes(guidLength);
            return new Guid(guidBytes);
        }

        /// <summary>
        /// Gets a random float between -1.0f and 1.0f
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static float NextFloatUniform(this Random rand)
        {
            return ((float)rand.NextDouble() - 0.5f) * 2f;
        }

        private static int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;
        
            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;
        
            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;
        
            if (targetWordCount == 0)
                return sourceWordCount;
        
            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];
        
            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++);
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++);
        
            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
        
                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }
        
            return distance[sourceWordCount, targetWordCount];
        }

        public static double Similarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;
        
            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }

        /*public static void GetExtIp()
        {
            return StartCoroutine(NetworkSetup());
        }
        private static IEnumerator NetworkSetup()
        {
            string externalIP;
            Network.Connect("127.0.0.1");

            while (Network.player.externalIP == "UNASSIGNED_SYSTEM_ADDRESS")
            {
                time += Time.deltaTime + 0.01f;

                if (time > 10)
                {
                    Debug.LogError(" Unable to obtain external ip: Are you sure your connected to the internet");
                }

                yield return null;
            }
            externalIP = Network.player.externalIP;
            Network.Disconnect();
            return externalIP;
        }*/
    }
}