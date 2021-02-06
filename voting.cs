using System;
using System.Collections.Generic;
using System.Text;

namespace DumaVoteCounter {
    public class Voting {
        public readonly int voteFor;
        public readonly int voteAgainst;
        public readonly int voteAbstained;

        public Voting(int voteFor = 49, int voteAgainst =0, int voteAbstained=0) {
            this.voteFor = voteFor;
            this.voteAgainst = voteAgainst;
            this.voteAbstained = voteAbstained;
        }
        //Проверка на ЕДИНОГЛАСНО
        public bool Edinoglasno => (voteAgainst == 0 && voteAbstained == 0);
        //проверка на корректность данных (недопустим отрицательный результат)
        public bool SomeThingWrong => voteFor < 0;
        public bool Accepted => (voteAgainst < voteFor);

    }
}
