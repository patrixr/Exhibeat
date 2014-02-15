using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exhibeat.Rhythm;
using Microsoft.Xna.Framework;

namespace Exhibeat.Gameplay
{
    class ScoreLogger : ITimeEventReciever
    {
        private List<float> graphValues = new List<float>();
        private Dictionary<userEvent, int> hitCounts = new Dictionary<userEvent, int>();
        private float current_completion = 1f;
        private int elapsed_ms = 0;
        private double score = 0;
        private int combo = 0;

        public ScoreLogger()
        {
            hitCounts[userEvent.NOTEBAD] = 0;
            hitCounts[userEvent.NOTEFAIL] = 0;
            hitCounts[userEvent.NOTEGOOD] = 0;
            hitCounts[userEvent.NOTENORMAL] = 0;
            hitCounts[userEvent.NOTEVERYGOOD] = 0;
        }

        public int GetHitCount(userEvent ev)
        {
            return hitCounts[ev];
        }

        public String getGradeTextureName()
        {/*
            int totalHits = 0;
            foreach (KeyValuePair<userEvent, int> kvp in hitCounts)
                totalHits += kvp.Value;
            float accuracy = totalHits;
            accuracy -= (float)hitCounts[userEvent.NOTEFAIL];
            accuracy -= (float)hitCounts[userEvent.NOTENORMAL] * 0.1f;
            accuracy -= (float)hitCounts[userEvent.NOTEGOOD] * 0.01f;
            accuracy -= (float)hitCounts[userEvent.NOTEBAD] * 0.2f;
            accuracy -= (float)hitCounts[userEvent.NOTEVERYGOOD] * 0f;
            */
            double percent = (getAccuracy());
            if (percent == 100)
                return "Sgrade";
            if (percent > 95)
                return "Agrade";
            if (percent > 90)
                return "Bgrade";
            if (percent > 85)
                return "Cgrade";
            else
                return "Dgrade";
        }

        public List<float> GetGraphValues()
        {
            return graphValues;
        }

        public float GetCurrentLife()
        {
            return current_completion;
        }

        public void LogCurrentLife()
        {
            graphValues.Add(current_completion);
        }

        public void NewSongEvent(songEvent ev, object param)
        {
        }

        public void Update(GameTime gameTime)
        {
            elapsed_ms += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed_ms > 1000)
            {
                LogCurrentLife();
                elapsed_ms = 0;
            }
        }


        /*300 + RoundDown(combo / 10)*80
         * 150 + RoundDown(combo / 10)*80
         * 75 + RoundDown(combo / 10)*80
         */

        public void NewUserEvent(userEvent ev, object param)
        {
            if (ev == userEvent.NOTEPRESSED || ev == userEvent.NOTERELEASED)
                return;
            if (ev == userEvent.NOTEFAIL)
            {
                current_completion -= 0.10f;
                combo = 0;
            }
            else if (ev == userEvent.NOTEGOOD)
            {
                current_completion += 0.015f;
                if (combo < 100)
                    score += ((300 + (combo / 10) * 80) / 2) / 2;
                else
                    score += 275;
                combo++;
            }
            else if (ev == userEvent.NOTENORMAL)
            {
                current_completion += 0.01f;
                if (combo < 100)
                    score += (300 + (combo / 10) * 80) / 2;
                else
                    score += 550;
                combo++;
            }
            else if (ev == userEvent.NOTEVERYGOOD)
            {
                current_completion += 0.02f;
                if (combo < 100)
                    score += 300 + (combo / 10) * 80;
                else
                    score += 1100;
                combo++;
            }
            if (current_completion > 1f)
                current_completion = 1f;
            else if (current_completion < 0f)
                current_completion = 0f;

            hitCounts[ev] += 1;
            graphValues.Add(current_completion);
            elapsed_ms = 0;
        }

        public double getAccuracy()
        {

           double accu = (50 * hitCounts[userEvent.NOTENORMAL] + 100 * hitCounts[userEvent.NOTEGOOD] + 300 * hitCounts[userEvent.NOTEVERYGOOD]);
           if (accu == 0)
               return 0.0;
           accu = accu / ((hitCounts[userEvent.NOTEFAIL] + hitCounts[userEvent.NOTENORMAL] + hitCounts[userEvent.NOTEGOOD] + hitCounts[userEvent.NOTEVERYGOOD]) * 300);
           return (accu * 100);
        }

        public double getScore()
        {
            return score;
        }

        public int getCombo()
        {
            return combo;
        }
    }
}
