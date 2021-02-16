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

        //Голосовали ЗА
        public int VoteFor => (Settings.peopleNumber - voteAgainst - voteAbstained);

        //проверка на корректность данных (недопустим отрицательный результат)
        public bool SomeThingWrong => VoteFor < 0;

        //Процент присутвующих
        public string PercentOfAttendance => Math.Round((double)(Settings.peopleNumber * 100 / Settings.MaxNumberOfDeputies)).ToString() + "%";

        //Принято не принято
        public bool Accepted => (voteAgainst < VoteFor);

    }
}
