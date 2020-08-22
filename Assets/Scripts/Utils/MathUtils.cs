using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sanicball.Extra {
	public static class MathUtils {

		public static float Map(float oldFrom, float oldTo, float value, float newFrom, float newTo){
			if (value <= newFrom) return newFrom;
			if (value >= newTo) return newTo;
			return (oldTo - oldFrom) * ((value - newFrom) / (newTo - newFrom)) + newFrom;
		}
	}
}