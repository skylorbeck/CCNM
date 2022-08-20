using System;

namespace SaveSystem
{
    [Serializable]
    public class TrackableStats
    {
        public int minionsKilled;
        public int bossesKilled;
        public int wheelsSpun;
        public int safesObtained;
        public int safesOpened;
        public int cardsObtained;
        public int cardsDismantled;
        public int gemsObtained;
        public int gemsUpgraded;
        public int gemsSocketed;
        public int gemsUnsockted;
        public int timesDied;
        public int timesRevived;
        public int creditsEarned;
        public int creditsSpent;
        public int creditsLost;
        public int egoEarned;
        public int egoSpent;
        public int egoLost;
        public int egoGained;
        public int[] cardSoulsGained = new int[7]; 
        public int[] cardSoulsSpent = new int[7]; 
        
        
        public TrackableStats()
        {
        }

        public TrackableStats(TrackableStats old)
        {
            minionsKilled = old.minionsKilled;
            bossesKilled = old.bossesKilled;
            wheelsSpun = old.wheelsSpun;
            safesOpened = old.safesOpened;
            cardsObtained = old.cardsObtained;
            cardsDismantled = old.cardsDismantled;
            gemsObtained = old.gemsObtained;
            gemsUpgraded = old.gemsUpgraded;
            gemsSocketed = old.gemsSocketed;
            gemsUnsockted = old.gemsUnsockted;
            timesDied = old.timesDied;
            timesRevived = old.timesRevived;
            creditsEarned = old.creditsEarned;
            creditsSpent = old.creditsSpent;
            creditsLost = old.creditsLost;
            egoEarned = old.egoEarned;
            egoSpent = old.egoSpent;
            egoLost = old.egoLost;
            egoGained = old.egoGained;
            cardSoulsGained = old.cardSoulsGained;
            cardSoulsSpent = old.cardSoulsSpent;
        }

        public void Add(TrackableStats stats)
        {
            minionsKilled += stats.minionsKilled;
            bossesKilled += stats.bossesKilled;
            wheelsSpun += stats.wheelsSpun;
            safesOpened += stats.safesOpened;
            cardsObtained += stats.cardsObtained;
            cardsDismantled += stats.cardsDismantled;
            gemsObtained += stats.gemsObtained;
            gemsUpgraded += stats.gemsUpgraded;
            gemsSocketed += stats.gemsSocketed;
            gemsUnsockted += stats.gemsUnsockted;
            timesDied += stats.timesDied;
            timesRevived += stats.timesRevived;
            creditsEarned += stats.creditsEarned;
            creditsSpent += stats.creditsSpent;
            creditsLost += stats.creditsLost;
            egoEarned += stats.egoEarned;
            egoSpent += stats.egoSpent;
            egoLost += stats.egoLost;
            egoGained += stats.egoGained;
            for (var i = 0; i < cardSoulsGained.Length; i++)
            {
                cardSoulsGained[i] += stats.cardSoulsGained[i];
            }
            for (var i = 0; i < cardSoulsSpent.Length; i++)
            {
                cardSoulsSpent[i] += stats.cardSoulsSpent[i];
            }
        }
    }
}