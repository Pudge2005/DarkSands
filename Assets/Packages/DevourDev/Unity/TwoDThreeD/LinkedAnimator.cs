using System;
using UnityEngine;

namespace DevourDev.Unity.TwoDThreeD
{
    public sealed class LinkedAnimator : MonoBehaviour, IAnimator
    {

        private TwoDThreeDView _activeView;


        public Animator ActiveAnimator => _activeView == null ? null : _activeView.Animator;


        internal TwoDThreeDView ActiveView
        {
            get => _activeView;
            set => SwitchView(value);
        }



        public bool GetBool(int hash)
        {
            var aa = ActiveAnimator;
            return aa != null && aa.GetBool(hash);
        }

        public float GetFloat(int hash)
        {
            var aa = ActiveAnimator;
            return aa == null ? default : aa.GetFloat(hash);
        }

        public int GetInt(int hash)
        {
            var aa = ActiveAnimator;
            return aa == null ? default : aa.GetInteger(hash);
        }

        public void SetBool(int hash, bool value)
        {
            var aa = ActiveAnimator;

            if (aa == null)
                return;

            aa.SetBool(hash, value);
        }

        public void SetFloat(int hash, float value)
        {
            var aa = ActiveAnimator;

            if (aa == null)
                return;

            aa.SetFloat(hash, value);
        }

        public void SetInt(int hash, int value)
        {
            var aa = ActiveAnimator;

            if (aa == null)
                return;

            aa.SetInteger(hash, value);
        }

        public void SetTrigger(int hash)
        {
            var aa = ActiveAnimator;

            if (aa == null)
                return;

            aa.SetTrigger(hash);
        }


        internal void SwitchView(TwoDThreeDView view)
        {
            //Animator prevAnimator = null;

            //if(_activeView != null)
            //{
            //    prevAnimator = _activeView.Animator;
            //}

            //if(prevAnimator != null)
            //{
            //    prevAnimator.
            //}

            if (view == _activeView)
                return;

            var prevView = _activeView;
            _activeView = view;

            if (_activeView != null)
            {
                view.gameObject.SetActive(true);
            }

            if (prevView != null)
            {
                if (view != null)
                {
                    if (prevView.Animator != null)
                    {
                        AnimationHelpers.SyncAnimators(prevView.Animator, view.Animator);
                    }
                }

                prevView.gameObject.SetActive(false);
            }
        }
    }
}
