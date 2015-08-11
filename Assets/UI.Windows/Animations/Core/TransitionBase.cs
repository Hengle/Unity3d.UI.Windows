﻿using UnityEngine;
using System.Collections;

namespace UnityEngine.UI.Windows.Animations {

	public class TransitionCameraAttribute : System.Attribute {
	};

	public class TransitionBase : ScriptableObject {

		[System.Serializable]
		public class ParametersBase {

			public const string MATERIAL_STRENGTH_NAME_DEFAULT = "_Value";

			public float inDelay;
			public float outDelay;

			public float inDuration;
			public float outDuration;
			
			public ME.Ease.Type inEase;
			public ME.Ease.Type outEase;
			
			public Material material;
			public string materialStrengthName;
			public bool materialLerpA = false;
			public bool materialLerpB = false;

			private Material materialInstance;

			public ParametersBase() {}

			public ParametersBase(ParametersBase defaults) {
				
				this.inDuration = defaults.inDuration;
				this.outDuration = defaults.outDuration;
				
				this.inDelay = defaults.inDelay;
				this.outDelay = defaults.outDelay;
				
				this.inEase = defaults.inEase;
				this.outEase = defaults.outEase;

				this.material = defaults.material;
				this.materialStrengthName = defaults.materialStrengthName;
				this.materialLerpA = defaults.materialLerpA;
				this.materialLerpB = defaults.materialLerpB;

				this.Setup(defaults);

			}
			
			public virtual void Setup(ParametersBase defaults) {

			}

			public string GetMaterialStrengthName() {

				var name = this.materialStrengthName;
				if (string.IsNullOrEmpty(name) == true) name = ParametersBase.MATERIAL_STRENGTH_NAME_DEFAULT;

				return name;

			}

			public void ResetMaterialInstance() {

				this.materialInstance = null;

			}

			public virtual Material GetMaterialInstance() {
				
				if (this.material == null) return null;
				if (this.materialInstance != null) return this.materialInstance;
				
				this.materialInstance = new Material(this.material);
				return this.materialInstance;
				
			}

		}

		public virtual ParametersBase GetDefaultInputParameters() {

			return null;

		}

		public virtual void SetInState(TransitionInputParameters parameters, WindowBase window, WindowComponentBase root) {
			
			var tag = this.GetInstanceID().ToString() + (root != null ? ("_" + root.GetInstanceID().ToString()) : string.Empty);
			if (TweenerGlobal.instance != null) TweenerGlobal.instance.removeTweens(tag);

		}
		
		public virtual void SetOutState(TransitionInputParameters parameters, WindowBase window, WindowComponentBase root) {

			var tag = this.GetInstanceID().ToString() + (root != null ? ("_" + root.GetInstanceID().ToString()) : string.Empty);
			if (TweenerGlobal.instance != null) TweenerGlobal.instance.removeTweens(tag);

		}
		
		public virtual void SetResetState(TransitionInputParameters parameters, WindowBase window, WindowComponentBase root) {

			var tag = this.GetInstanceID().ToString() + (root != null ? ("_" + root.GetInstanceID().ToString()) : string.Empty);
			if (TweenerGlobal.instance != null) TweenerGlobal.instance.removeTweens(tag);

		}

		public void Play(WindowBase window, bool forward, System.Action callback) {

			this.Play(window, null, null, forward, callback);

		}

		public virtual void Set(WindowBase window, TransitionInputParameters parameters, WindowComponentBase root, bool forward, float value) {



		}

		public void Play(WindowBase window, TransitionInputParameters parameters, WindowComponentBase root, bool forward, System.Action callback) {

			var delay = this.GetDelay(parameters, forward);
			var tag = this.GetInstanceID().ToString() + (root != null ? ("_" + root.GetInstanceID().ToString()) : string.Empty);
			
			if (TweenerGlobal.instance != null) TweenerGlobal.instance.removeTweens(tag);

			if (delay > 0f && TweenerGlobal.instance != null) {

				TweenerGlobal.instance.addTween(this, delay, 0f, 0f).tag(tag).onComplete(() => {

					this.OnPlay(window, tag, parameters, root, forward, callback);

				}).onCancel((obj) => {

					if (callback != null) callback();

				});

			} else {

				this.OnPlay(window, tag, parameters, root, forward, callback);

			}

		}

		public virtual void OnInit() {}

		public virtual void OnPlay(WindowBase window, object tag, TransitionInputParameters parameters, WindowComponentBase root, bool forward, System.Action callback) {}

		public virtual float GetDelay(TransitionInputParameters parameters, bool forward) {
			
			var param = this.GetParams<ParametersBase>(parameters);
			
			if (forward == true) {
				
				return param.inDelay;
				
			}
			
			return param.outDelay;
			
		}
		
		public virtual float GetDuration(TransitionInputParameters parameters, bool forward) {
			
			var param = this.GetParams<ParametersBase>(parameters);
			
			if (forward == true) {
				
				return param.inDuration;
				
			}
			
			return param.outDuration;
			
		}

		public T GetParams<T>(TransitionInputParameters parameters) where T : ParametersBase {
			
			T param = null;
			if (parameters == null || parameters.useDefault == true) {

				param = this.GetDefaultInputParameters() as T;
				
			} else {
				
				param = parameters.GetParameters<T>();
				
			}
			
			return param;
			
		}
		
		public virtual void OnRenderTransition(WindowBase window, RenderTexture source, RenderTexture destination) {}

		public virtual bool IsValid(WindowBase window) { return false; }

		public virtual void SetupCamera(Camera camera) {}

	}

}