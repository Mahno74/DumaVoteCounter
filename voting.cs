using System;
using System.Collections.Generic;
using System.Text;

namespace DumaVoteCounter {
    public class Voting {
        public readonly int voteAgainst;
        public readonly int voteAbstained;

        public Voting( int voteAgainst =0, int voteAbstained=0) {
            this.voteAgainst = voteAgainst;
            this.voteAbstained = voteAbstained;
        }
        //Проверка на ЕДИНОГЛАСНО
        public bool Edinoglasno => (voteAgainst == 0 && voteAbstained == 0);

        public int voteFor => (Settings.peopleNumber - voteAgainst - voteAbstained);
        //проверка на корректность данных (недопустим отрицательный результат)
        public bool SomeThingWrong => voteFor < 0;

        public bool Accepted => (voteAgainst < voteFor);

    }
}
