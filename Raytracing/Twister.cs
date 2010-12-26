using System;

namespace Raytracing {
    public class Twister {
        private const int M = 397;
        private const int MtRandN = 624;
        private const ulong MatrixA = 0x9908b0df;
        private const ulong UpperMask = 0x80000000;
        private const ulong LowerMask = 0x7FFFFFFF;
        private const ulong TemperingMaskB = 0x9d2c5680;
        private const ulong TemperingMaskC = 0xefc60000;

        private readonly ulong[] _mt = new ulong[MtRandN];
        private int _mti;

        private Random _random = new Random();

        private static readonly ulong[] Mag01 = new ulong[] { 0x0, MatrixA };

        public Twister(ulong seed) {
            Seed(seed > 0 ? seed : 0xf2710812);
        }

        public void Seed(ulong seed) {
            _mt[0] = seed & 0xffffffff;
            for (_mti = 1; _mti < MtRandN; _mti++) _mt[_mti] = (69069 * _mt[_mti - 1]) & 0xffffffff;
            ulong s = 373737;
            for (_mti = 1; _mti < MtRandN; _mti++) {
                _mt[_mti] ^= s;
                s = s*5531 + 81547;
                s ^= (s >> 9) ^ (s << 19);
            }
        }

        public double Rand {
            get {
                return (double) _random.NextDouble();
                //return ((double)RandL * 2.3283064370807974e-10f);
            }
        }

        public ulong RandL {
            get {
                ulong y;
                if (_mti >= MtRandN) 
	            {
                    int kk;
                    for (kk=0;kk<MtRandN-M;kk++) 
		            {
                        y = (_mt[kk]&UpperMask)|(_mt[kk+1]&LowerMask);
                        _mt[kk] = _mt[kk+M] ^ (y >> 1) ^ Mag01[y & 0x1];
                    }
                    for (;kk<MtRandN-1;kk++) 
		            {
                        y = (_mt[kk]&UpperMask)|(_mt[kk+1]&LowerMask);
                        _mt[kk] = _mt[kk + (M - MtRandN)] ^ (y >> 1) ^ Mag01[y & 0x1];
                    }
                    y = (_mt[MtRandN-1]&UpperMask)|(_mt[0]&LowerMask);
                    _mt[MtRandN - 1] = _mt[M - 1] ^ (y >> 1) ^ Mag01[y & 0x1];
                    _mti = 0;
                }
                y = _mt[_mti++];
                y ^= y >> 11;
                y ^= y << 7 & TemperingMaskB;
                y ^= y << 15 & TemperingMaskC;
                y ^= y >> 18;
                return y;
            }
        }
    }
}
