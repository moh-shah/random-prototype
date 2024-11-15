using CardMatch.Models;
using UnityEngine;

namespace CardMatch.Gameplay
{
    public class MatchEngineManager
    {
        
    }
    public class DependencyManager : MonoBehaviour
    {
        public static DependencyManager Instance;
        
        public SpritesProvider spritesProvider;
        public CardFactory cardFactory;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
 
    }
}