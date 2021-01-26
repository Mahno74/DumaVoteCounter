using System;
using System.Collections.Generic;
using System.Text;

namespace DumaVoteCounter {
    class Voting {
        private int numberOfPeople;
        private int voteFor;
        private int voteAgainst;
        private int voteAbstained;

        public Voting(int numberOfPeople, int voteFor, int voteAgainst =0, int voteAbstained=0) {
            this.numberOfPeople = numberOfPeople;
            this.voteFor = voteFor;
            this.voteAgainst = voteAgainst;
            this.voteAbstained = voteAbstained;
        }
    }
}
