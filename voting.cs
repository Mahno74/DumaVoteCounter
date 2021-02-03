using System;
using System.Collections.Generic;
using System.Text;

namespace DumaVoteCounter {
    public class Voting {
        public readonly int numberOfPeople;
        public readonly int voteFor;
        public readonly int voteAgainst;
        public readonly int voteAbstained;

        public Voting(int numberOfPeople, int voteFor, int voteAgainst =0, int voteAbstained=0) {
            this.numberOfPeople = numberOfPeople;
            this.voteFor = voteFor;
            this.voteAgainst = voteAgainst;
            this.voteAbstained = voteAbstained;
        }
        public bool Edinoglasno() {
            return voteAgainst == 0 && voteAbstained == 0;
        }
    }
}
