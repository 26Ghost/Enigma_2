using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma_2
{
    public class Data
    {

        public string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string _rotorI_value = "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
        public string _rotorII_value = "AJDKSIRUXBLHWTMCQGZNPYFVOE";
        public string _rotorIII_value = "BDFHJLCPRTXVZNYEIWGAKMUSQO";
        public char[] _reflector_A = "EJMZALYXVBWFCRQUONTSPIKHGD".ToCharArray();

        public Dictionary<char, char> _rotorI = new Dictionary<char, char>();
        public Dictionary<char, char> _rotorII = new Dictionary<char, char>();
        public Dictionary<char, char> _rotorIII = new Dictionary<char, char>();
        public Dictionary<char, char> _reflector = new Dictionary<char, char>();

        public List<Dictionary<char, char>> _rotors = new List<Dictionary<char, char>>();
        public List<string> _rotor_values = new List<string>();

        public bool interactivity = true;
        public int last_index_1 = -1;
        public int last_index_2 = -1;
        public int last_index_3 = -1;

        public Data()
        {
            foreach (var el in _alphabet.Select((value, index) => new { value, index }))
            {
                _rotorI.Add(el.value, _rotorI_value[el.index]);
                _rotorII.Add(el.value, _rotorII_value[el.index]);
                _rotorIII.Add(el.value, _rotorIII_value[el.index]);
                _reflector.Add(el.value, _reflector_A[el.index]);
            }

            _rotors.Add(_rotorI);
            _rotors.Add(_rotorII);
            _rotors.Add(_rotorIII);
            _rotors.Add(_reflector);

            _rotor_values.Add(_rotorI_value);
            _rotor_values.Add(_rotorII_value);
            _rotor_values.Add(_rotorIII_value);
        }

        public void rotor_rebuild(int offset, int rotor_index)
        {
            string new_value = _rotor_values[rotor_index].Substring(26 - offset, offset) + _rotor_values[rotor_index].Substring(0, 26 - offset);
            string new_key = _alphabet.Substring(26 - offset, offset) + _alphabet.Substring(0, 26 - offset);
            _rotors[rotor_index].Clear();

            foreach (var el in new_key.Select((value, index) => new { value, index }))
            {
                _rotors[rotor_index].Add(el.value, new_value[el.index]);
            }
        }

    }
}
