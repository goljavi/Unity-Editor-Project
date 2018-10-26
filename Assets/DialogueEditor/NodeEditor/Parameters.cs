using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters {

	private Dictionary<string, int> intParameters = new Dictionary<string, int>();
	private Dictionary<string, float> floatParameters = new Dictionary<string, float>();
	private Dictionary<string, bool> boolParameters = new Dictionary<string, bool>();

	//Lista de nombres de parametros de cada tipo
	public List<string> IntParametersNames {
		get
		{
			return new List<string>(intParameters.Keys);
		}
	}
	public List<string> FloatParametersNames {
		get
		{
			return new List<string>(floatParameters.Keys);
		}
	}
	public List<string> BoolParametersNames {
		get
		{
			return new List<string>(boolParameters.Keys);
		}
	}

	public enum ParameterType { Int, Float, Bool };

	//Devolver el valor dado el nombre del parametro
	//Si se necesita verificar si se obtuvo un valor valido, usar el que tiene "out bool"
	//int
	public int GetInt(string parameterName, out bool success) {
		if (intParameters.ContainsKey(parameterName))
		{
			success = true;
			return intParameters[parameterName];
		} else
		{
			success = false;
			return default(int);
		}
	}
	public int GetInt(string parameterName) {
		bool voidVar;
		return (GetInt(parameterName, out voidVar));
	}

	//float
	public float GetFloat(string parameterName, out bool success) {
		if (floatParameters.ContainsKey(parameterName))
		{
			success = true;
			return floatParameters[parameterName];
		} else
		{
			success = false;
			return default(float);
		}
	}
	public float GetFloat(string parameterName) {
		bool voidVar;
		return GetFloat(parameterName, out voidVar);
	}

	//bool
	public bool GetBool(string parameterName, out bool success) {
		if (boolParameters.ContainsKey(parameterName))
		{
			success = true;
			return boolParameters[parameterName];
		} else
		{
			success = false;
			return default(bool);
		}
	}
	public bool GetBool(string parameterName) {
		bool voidVar;
		return (GetBool(parameterName, out voidVar));
	}

	//Devuelve un parametro predeterminado, si existen
	public string DefaultIntName {
		get
		{
			if (IntParametersNames.Count > 0)
				return IntParametersNames[0];
			return "";
		}
	}
	public string DefaultFloatName {
		get
		{
			if (FloatParametersNames.Count > 0)
				return FloatParametersNames[0];
			return "";
		}
	}
	public string DefaultBoolName {
		get
		{
			if (BoolParametersNames.Count > 0)
				return BoolParametersNames[0];
			return "";
		}
	}

	//Crear un parametro
	public void AddInt(string name) {
		if (!intParameters.ContainsKey(name))
			intParameters.Add(name, 0);
	}
	public void AddFloat(string name) {
		if (!floatParameters.ContainsKey(name))
			floatParameters.Add(name, 0f);
	}
	public void AddBool(string name) {
		if (!boolParameters.ContainsKey(name))
			boolParameters.Add(name, false);
	}

	//Asignar un parametro
	public void SetInt(string name, int value) {
		if (intParameters.ContainsKey(name))
			intParameters[name] = value;
	}
	public void Setfloat(string name, float value) {
		if (floatParameters.ContainsKey(name))
			floatParameters[name] = value;
	}
	public void SetBool(string name, bool value) {
		if (boolParameters.ContainsKey(name))
			boolParameters[name] = value;
	}

	//Renombrar un parametro
	public void RenameParameter(string oldName, string newName, ParameterType type) {
		switch (type)
		{
			case ParameterType.Int:
				if (intParameters.ContainsKey(oldName))
				{
					intParameters[newName] = intParameters[oldName];
					intParameters.Remove(oldName);
				}
				break;
			case ParameterType.Float:
				if (floatParameters.ContainsKey(oldName))
				{
					floatParameters[newName] = floatParameters[oldName];
					floatParameters.Remove(oldName);
				}
				break;
			case ParameterType.Bool:
				if (boolParameters.ContainsKey(oldName))
				{
					boolParameters[newName] = boolParameters[oldName];
					boolParameters.Remove(oldName);
				}
				break;
		}
	}

	//Obtener data serializada
	public string GetData() {
		//int
		List<string> intN = new List<string>();
		List<int> intV = new List<int>();
		foreach (var item in intParameters)
		{
			intN.Add(item.Key);
			intV.Add(item.Value);
		}

		//float
		List<string> floatN = new List<string>();
		List<float> floatV = new List<float>();
		foreach (var item in floatParameters)
		{
			floatN.Add(item.Key);
			floatV.Add(item.Value);
		}

		//bool
		List<string> boolN = new List<string>();
		List<bool> boolV = new List<bool>();
		foreach (var item in boolParameters)
		{
			boolN.Add(item.Key);
			boolV.Add(item.Value);
		}

		return JsonUtility.ToJson(new ParametersData
		{
			intNames = intN,
			intValues = intV,
			floatNames = floatN,
			floatValues = floatV,
			boolNames = boolN,
			boolValues = boolV
		});
	}

	//Asignar data a partir de data serializada
	public void SetData(string data) {
		ParametersData converted = JsonUtility.FromJson<ParametersData>(data);
		//ints
		intParameters.Clear();
		int c = converted.intNames.Count <= converted.intValues.Count ?
			converted.intNames.Count : converted.intValues.Count; //uses minimum count in case of disparity
		for (int i = 0; i < c; i++)
			intParameters.Add(converted.intNames[i], converted.intValues[i]);

		//floats
		floatParameters.Clear();
		c = converted.floatNames.Count <= converted.floatValues.Count ?
			converted.floatNames.Count : converted.floatValues.Count; //uses minimum count in case of disparity
		for (int i = 0; i < c; i++)
			floatParameters.Add(converted.floatNames[i], converted.floatValues[i]);

		//bools
		boolParameters.Clear();
		c = converted.boolNames.Count <= converted.boolValues.Count ?
			converted.boolNames.Count : converted.boolValues.Count; //uses minimum count in case of disparity
		for (int i = 0; i < c; i++)
			boolParameters.Add(converted.boolNames[i], converted.boolValues[i]);
	}
}

[System.Serializable]
public class ParametersData {
	public List<string> intNames;
	public List<int> intValues;
	public List<string> floatNames;
	public List<float> floatValues;
	public List<string> boolNames;
	public List<bool> boolValues;
}