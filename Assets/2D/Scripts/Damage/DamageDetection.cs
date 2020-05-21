﻿using Aquaivy.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KnightAdventure
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DamageDetection : MonoBehaviour
    {
        private Character character;

        void Awake()
        {
        }

        public void SetData(Character character, Rect rect, int aliveTime)
        {
            this.character = character;

            SetDetectionRect(rect);
            if (aliveTime > 0)
            {
                AliveCountdown(aliveTime);
            }
        }

        public void SetDetectionRect(Rect rect)
        {
            var box = GetComponent<BoxCollider2D>();
            box.offset = rect.position;
            box.size = rect.size;
        }

        private DelayTask taskAliveCountdown;

        private void AliveCountdown(int aliveTime)
        {
            taskAliveCountdown = DelayTask.Invoke(() =>
            {
                if (this.gameObject != null)
                {
                    GameObject.Destroy(this.gameObject);
                }
            }, aliveTime);
        }

        private void OnDestroy()
        {
            taskAliveCountdown?.Release();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log($"DamageDetection  attacker={attacker.gameObject.name}    hit={collision.gameObject.name}");

            var life = collision.GetComponent<LifeBehaviour>();
            if (life != null)
            {
                if (collision.GetComponent<Character>() != character)
                {
                    life.ReduceHP(character.Attack.AttackDamage);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //Debug.Log($"OnTriggerExit2D  {collision.gameObject.name}");
        }
    }
}