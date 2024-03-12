using Code.Bomb;
using System.Collections.Generic;

namespace Code.Extension
{
    public static class TakeDamageSortExtension
    {
        public static void ToTakeDamageSort(this List<BombAttack.TakeDamageInfo> takeDamageInfos)
        {
            int _n = takeDamageInfos.Count;
            bool _swapped;

            do
            {
                _swapped = false;
                for (int i = 1; i < _n; i++)
                {
                    if (takeDamageInfos[i - 1].Distance > takeDamageInfos[i].Distance)
                    {
                        BombAttack.TakeDamageInfo temp = takeDamageInfos[i - 1];
                        takeDamageInfos[i - 1] = takeDamageInfos[i];
                        takeDamageInfos[i] = temp;

                        _swapped = true;
                    }
                }
                _n--;
            } while (_swapped);
        }
    }
}