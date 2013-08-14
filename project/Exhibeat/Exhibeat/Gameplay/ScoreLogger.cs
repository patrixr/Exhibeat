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

        public void NewUserEvent(userEvent ev, object param)
        {
            if (ev == userEvent.NOTEPRESSED || ev == userEvent.NOTERELEASED)
                return;
            if (ev == userEvent.NOTEFAIL)
                current_completion -= 0.01f;
            else if (ev == userEvent.NOTEGOOD)
                current_completion += 0.015f;
            else if (ev == userEvent.NOTENORMAL)
                current_completion += 0.01f;
            else if (ev == userEvent.NOTEVERYGOOD)
                current_completion += 0.02f;

            if (current_completion > 1f)
                current_completion = 1f;
            else if (current_completion < 0f)
                current_completion = 0f;

            hitCounts[ev] += 1;
            graphValues.Add(current_completion);
            elapsed_ms = 0;
        }
    }
}
