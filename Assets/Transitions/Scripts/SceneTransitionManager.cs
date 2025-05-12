using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneTransition
{

    public class SceneTransitionManager : Singleton<SceneTransitionManager>
    {
        protected override bool DestroyOnLoad => false;

        public Material material;

        public Transition transition;


        private bool inTransition;



        //Illustration sequence patch
        public bool useCustomFunction = false;
        public delegate void CustomFunction();
        public CustomFunction customFunction = () => { };

        private void Start()
        {
            if (DestroyIfInitialised(this)) return;

            EnsureInitialised();
            ResetMaterial();
        }
        private void OnDestroy()
        {
            if (ImTheOne(this))
                ResetMaterial();
        }

        [Button]
        void ResetMaterial()
        {
            material.SetFloat("_time", 0);
        }


        public bool IsAlreadyTransitioning()
        {
            return inTransition;
        }

        //TODO: mejorar la forma de indexar las escenas
        //public void ChangeScene(SceneIndex sceneIndex)
        //{
        //    ChangeScene((int)sceneIndex);
        //}

        public void ChangeScene(string sceneName)
        {
            if (inTransition)
            {
                throw new UnityException("There is already a transition in progress");
            }

            StartCoroutine(SceneSwap(sceneName));
        }

        public void CancelCurrentTransition()
        {
            if (!inTransition)
            {
                throw new UnityException("There is no active transition");
            }

            StopAllCoroutines();
            inTransition = false;
        }

        private IEnumerator SceneSwap(string sceneName)
        {
            inTransition = true;

            material.SetTexture("_background", transition.background);
            material.SetColor("_backgroundColor", transition.backgroundColor);
            material.SetTexture("_transitionGradient", transition.In.gradientMask);

            //Fade in
            material.SetFloat("_time", 0);
            material.SetFloat("_invert", transition.In.invert ? 1 : 0);


            if (transition.In.enabled)
                for (float i = 0; i < transition.In.duration; i += Time.deltaTime)
                {

                    material.SetFloat("_time", transition.In.interpolation.Interpolate(i / transition.In.duration));
                    yield return null;
                }

            material.SetFloat("_time", 1);
            material.SetFloat("_inverted", transition.Out.invert ? 1 : 0);

            material.SetTexture("_transitionGradient", transition.Out.gradientMask);
            //Load Scene
            if(!useCustomFunction)
            {
                SceneManager.LoadScene(sceneName);
            }
            else {

                customFunction();
            }


            if (transition.middleScreenDuration > 0)
                yield return new WaitForSeconds(transition.middleScreenDuration);

            //Wait screen

            //Fade out
            if (transition.Out.enabled)
                for (float i = transition.Out.duration; i > 0; i -= Time.deltaTime)
                {
                    material.SetFloat("_time", transition.Out.interpolation.Interpolate(i / transition.Out.duration));
                    yield return null;
                }

            material.SetFloat("_time", 0);


            inTransition = false;
        }


        public bool IsTransitionComplete()
        {
            return !inTransition;
        }

        [Button]
        public void ChangeSceneToSandbox()
        {
            ChangeScene("Sandbox");
        }

        [Button]
        public void ChangeSceneToSample()
        {
            ChangeScene("SampleScene");
        }

    }

}