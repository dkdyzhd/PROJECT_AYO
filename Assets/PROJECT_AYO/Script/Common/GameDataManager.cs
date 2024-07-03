using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Instance { get; private set; } = null;

        public List<Item> ItemDataList = new List<Item>();

        public bool isGameOver;

        public CharacterBase playercharacter;

        private void Start()
        {
            playercharacter.onCharacterDead += GameOverCheck;   //onCharacterDead에 체인을 거는것
            
        }

        public void GameOverCheck()
        {
            //To do: 
            Debug.Log("GameOver");
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
