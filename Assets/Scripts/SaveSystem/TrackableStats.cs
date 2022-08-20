using System;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class TrackableStats
    {
        public int minionsKilled = 0;
        public int bossesKilled = 0;
        public int wheelsSpun = 0;
        public int safesOpened = 0;
        public int cardsObtained = 0;
        public int cardsDismantled = 0;
        public int gemsObtained = 0;
        public int gemsUpgraded = 0;
        public int gemsSocketed = 0;
        public int gemsUnsockted = 0;
        public int timesDied = 0;
        public int timesRevived = 0;
        public int creditsEarned = 0;
        public int creditsSpent = 0;
        public int creditsLost = 0;
        public int egoEarned = 0;
        public int egoSpent = 0;
        public int egoLost = 0;
        public int egoGained = 0;
        public int[] cardSoulsGained = new int[7]; 
        public int[] cardSoulsSpent = new int[7]; 
        
        
        public TrackableStats()
        {
        }

        public TrackableStats(TrackableStats old)
        {
            this.minionsKilled = old.minionsKilled;
            this.bossesKilled = old.bossesKilled;
            this.wheelsSpun = old.wheelsSpun;
            this.safesOpened = old.safesOpened;
            this.cardsObtained = old.cardsObtained;
            this.cardsDismantled = old.cardsDismantled;
            this.gemsObtained = old.gemsObtained;
            this.gemsUpgraded = old.gemsUpgraded;
            this.gemsSocketed = old.gemsSocketed;
            this.gemsUnsockted = old.gemsUnsockted;
            this.timesDied = old.timesDied;
            this.timesRevived = old.timesRevived;
            this.creditsEarned = old.creditsEarned;
            this.creditsSpent = old.creditsSpent;
            this.creditsLost = old.creditsLost;
            this.egoEarned = old.egoEarned;
            this.egoSpent = old.egoSpent;
            this.egoLost = old.egoLost;
            this.egoGained = old.egoGained;
            this.cardSoulsGained = old.cardSoulsGained;
            this.cardSoulsSpent = old.cardSoulsSpent;
        }
    }
}