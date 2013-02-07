using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ExhiBeat.KeyReader
{
    class KeyReader : IKeyReader
    {
        private bool[]      _curState;
        private List<Key>  _keyEvents;

        public KeyReader()
        {
            int i;

            _curState = new bool[7];
            _keyEvents = new List<Key>();
            for (i = 0; i < _curState.Length; i++)
                _curState[i] = false;
        }

        private void createNewKeyEvent(int pos, keyType type)
        {
            Key key = new Key();

            key.pos = pos;
            key.type = type;
            _keyEvents.Add(key);
        }

        private void checkKey(KeyboardState state, Keys key, int pos)
        {
           // Console.WriteLine("Check");
            if (state.IsKeyDown(key))
            {
             //   Console.WriteLine("key down !");
                if (_curState[pos] != true)
                {
                    _curState[pos] = true;
                    createNewKeyEvent(pos, keyType.pressed);
                }
            }
            else if (_curState[pos] == true)
            {
                _curState[pos] = false;
                createNewKeyEvent(pos, keyType.realeased);
            }
        }

        private void refresh()
        {
            KeyboardState state = Keyboard.GetState();

            checkKey(state, Keys.S, 0);
            checkKey(state, Keys.E, 1);
            checkKey(state, Keys.D, 2);
            checkKey(state, Keys.X, 3);
            checkKey(state, Keys.Z, 4);
            checkKey(state, Keys.A, 5);
            checkKey(state, Keys.W, 6);
        }

        public Key getNextKeyEvent()
        {
            Key key;

            refresh();
            if (_keyEvents.Count == 0)
                return null;
            key = _keyEvents[0];
            _keyEvents.RemoveAt(0);
            return key;
        }
    }
}
