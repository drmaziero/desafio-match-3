using System;
using UnityEngine;

namespace GameLogic.Services
{
    public class LifeService
    {
        public event Action<int> LifeChanged; 
        
        private const string _targetTimeKey = "targetLifeKey";
        private const string _lifeKey = "lifeKey";
        
        private const int _maxLife = 5;
        private readonly TimeSpan _refreshLifeTime = TimeSpan.FromMinutes(30);
        
        public int Life { get; private set; }
        public int MaximumLife => _maxLife;
        private DateTime _targetTime;

        public LifeService()
        {
            Life = PlayerPrefs.GetInt(_lifeKey, _maxLife);
            _targetTime = LoadTargetTime();
            VerifyIncreaseLife();
        }

        public bool HasLife()
        {
            VerifyIncreaseLife();
            return Life > 0;
        }
        
        public bool TryDecreaseLife()
        {
            VerifyIncreaseLife();
            if (Life <= 0)
                return false;

            bool wasFull = Life == _maxLife;
            Life--;
            SaveLife();
            
            if (wasFull || _targetTime == DateTime.MinValue)
                StartLifeTimer();
            
            LifeChanged?.Invoke(Life);
            return true;
        }

        public void AddLife(int amount = 1)
        {
            if (amount <= 0)
                return;
            
            VerifyIncreaseLife();
            int previousLife = Life;
            Life = Mathf.Min(Life + amount, _maxLife);

            if (Life >= _maxLife)
                ClearTargetTime();
            else if (_targetTime == DateTime.MinValue)
                StartLifeTimer();
            
            SaveLife();
            if (Life != previousLife)
                LifeChanged?.Invoke(Life);
        }
        
        public void VerifyIncreaseLife()
        {
            if (Life >= _maxLife)
            {
                ClearTargetTime();
                return;
            }

            if (_targetTime == DateTime.MinValue)
            {
                StartLifeTimer();
                return;
            }
            
            DateTime now = DateTime.UtcNow;

            if (now < _targetTime)
                return;

            int previousLife = Life;

            while (Life < _maxLife && now >= _targetTime)
            {
                Life++;
                if (Life < _maxLife)
                    _targetTime += _refreshLifeTime;
            }

            if (Life >= _maxLife)
                ClearTargetTime();
            else
                SaveTargetTimer();

            if (Life != previousLife)
            {
                SaveLife();
                LifeChanged?.Invoke(Life);
            }
        }

        public TimeSpan? GetTimeUntilNextLife()
        {
            VerifyIncreaseLife();

            if (Life >= _maxLife || _targetTime == DateTime.MinValue)
                return null;

            TimeSpan remaining = _targetTime - DateTime.UtcNow;

            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }

        private void StartLifeTimer()
        {
            _targetTime = DateTime.UtcNow + _refreshLifeTime;
            SaveTargetTimer();
        }

        private void ClearTargetTime()
        {
            if (_targetTime == DateTime.MinValue)
                return;
            
            _targetTime = DateTime.MinValue;
            SaveTargetTimer();
        }

        private void SaveLife()
        {
            PlayerPrefs.SetInt(_lifeKey, Mathf.Clamp(Life, 0, _maxLife));
            PlayerPrefs.Save();
        }

        private void SaveTargetTimer()
        {
            PlayerPrefs.SetString(_targetTimeKey, _targetTime.Ticks.ToString());
            PlayerPrefs.Save();
        }

        private DateTime LoadTargetTime()
        {
            string savedValue = PlayerPrefs.GetString(_targetTimeKey, DateTime.MinValue.Ticks.ToString());

            if (!long.TryParse(savedValue, out long ticks))
            {
                return DateTime.MinValue;
            }

            try
            {
                return new DateTime(ticks, DateTimeKind.Utc);
            }
            catch (ArgumentOutOfRangeException)
            {
                return DateTime.MinValue;
            }
        }
    }
}