using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ComparativeNode : BaseNode, INeedsChildren {

	private BaseNode[] children = new BaseNode[2]; //Los nodos hijos a los que se va a elegir
	private int[] childrenIDs = new int[2]; //IDs de los hijos para futura asignacion

	//Tipo de comparacion activa
	public enum ComparisonType { Float, Int, Bool }
	private ComparisonType activeType;

	//Operacion comparativa activa
	public enum ComparisonOperator { Equals, NotEqual, Lesser, LesserEquals, Greater, GreaterEquals }
	public ComparisonOperator activeOperator;


	//Valores con los que se van a comparar
	private string[] parameterNames = new string[2] { "", "" };
	private Parameters parameterSource;

	public override string GetNodeType { get { return "Comparison"; } }

	public Parameters ParameterSource { get { return parameterSource; } set { parameterSource = value; } }

	public override void DrawNode() {
		activeType = (ComparisonType)EditorGUILayout.Popup((int)activeType,
			new string[] { "Float", "Int", "Bool" }, new GUIStyle());
		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.EndHorizontal();
	}

	//Obtencion del nodo correspondiente al resultado. 
	//Tambien se puede acceder con un booleano externo (si por alguna razon se quisiera)
	public BaseNode GetResult(bool successful) {
		if (!HasChildren())
		{
			throw new System.Exception("One or both children nodes are missing. Make sure to assign them properly.");
		}
		return successful ? children[0] : children[1];
	}
	//Como se usara por default.
	public BaseNode GetResult() {
		return GetResult(Compare());
	}

	//Asignacion de hijos correspondiente a verdadero(0) o falso(1). Si es negativo busca si tiene el ID de child
	//De INeedsChildren
	public void AssignChild(BaseNode child, int childPosition) {
		//si no es un valor definido
		if (!ValidIndex(childPosition))
		{
			//positivo y fuera de rango, error
			if (childPosition > 1)
				throw new System.Exception("Invalid child position. Child must be 0(true) or 1(false), or negative to attempt automatic assignment");

			//Asigna hijo si lo tiene guardado de inicializacion
			if (child.id == childrenIDs[0])
				children[0] = child;
			if (child.id == childrenIDs[1])
				children[1] = child;

			//Si el valor es valido, asignarlo. Para cuando se agrega manualmente.
		} else
		{
			if (children[childPosition] != null)
				children[childPosition].SetParent(null);

			children[childPosition] = child;
		}
	}

	//Tiene hijos validos. INeedsChildren
	public bool HasChildren() {
		return (children[0] != null && children[1] != null);
	}


	//En caso de ser una asignacion por inicializacion
	public override BaseNode SetParent(BaseNode value) {
		if (children[0] == value || children[1] == value)
			return base.SetParent(value);
		return this;
	}

	//forma alternativa de setear hijo, mas directo para asignacion manual.
	public BaseNode SetChild(BaseNode node, bool correspondingCase) {
		int i = correspondingCase ? 0 : 1;
		children[i] = node;
		return this;
	}

	//Ejecuta la comparacion correspondiente
	public bool Compare() {
		switch (activeType)
		{
			case ComparisonType.Float:
				return CompareFloat();
			case ComparisonType.Int:
				return CompareInt();
			case ComparisonType.Bool:
				return parameterSource.GetBool(parameterNames[0]);
			default:
				Debug.LogWarning("Invalid Comparison Type Enum at " + this.ToString());
				return false;
		}
	}

	//Comparacion de floats
	private bool CompareFloat() {

		float float1 = parameterSource.GetFloat(parameterNames[0]);
		float float2 = parameterSource.GetFloat(parameterNames[1]);
		switch (activeOperator)
		{
			case ComparisonOperator.Equals:
				return float1 == float2;
			case ComparisonOperator.NotEqual:
				return !(float1 == float2);
			case ComparisonOperator.Lesser:
				return float1 < float2;
			case ComparisonOperator.LesserEquals:
				return float1 <= float2;
			case ComparisonOperator.Greater:
				return float1 > float2;
			case ComparisonOperator.GreaterEquals:
				return float1 >= float2;
			default:
				Debug.LogWarning("Invalid Comparison Operator Enum at " + this.ToString());
				return false;
		}
	}

	//comparacion de ints
	private bool CompareInt() {
		float int1 = parameterSource.GetInt(parameterNames[0]);
		float int2 = parameterSource.GetInt(parameterNames[1]);
		switch (activeOperator)
		{
			case ComparisonOperator.Equals:
				return int1 == int2;
			case ComparisonOperator.NotEqual:
				return !(int1 == int2);
			case ComparisonOperator.Lesser:
				return int1 < int2;
			case ComparisonOperator.LesserEquals:
				return int1 <= int2;
			case ComparisonOperator.Greater:
				return int1 > int2;
			case ComparisonOperator.GreaterEquals:
				return int1 >= int2;
			default:
				Debug.LogWarning("Invalid Comparison Operator Enum at " + this.ToString());
				return false;
		}
	}

	private bool ValidIndex(int value) { if (value == 0 || value == 1) return true; else return false; }

	//Asignacion y obtencion de data
	public override void SetNodeData(string data) {
		ComparativeNodeData converted = JsonUtility.FromJson<ComparativeNodeData>(data);
		childrenIDs = converted.childrenIDs;
		activeType = converted.comparisonType;
		activeOperator = converted.comparisonOperator;
		parameterNames = converted.parameterName;
	}

	public override string GetNodeData() {
		return JsonUtility.ToJson(new ComparativeNodeData
		{

			childrenIDs = new int[2] {
				children[0] ==null ? -1 : children[0].id,
				children[1] == null ? -1 : children[1].id
			},
			comparisonType = activeType,
			comparisonOperator = activeOperator,
			parameterName = parameterNames
		});
	}

}

//Datos del nodo.
[System.Serializable]
public struct ComparativeNodeData {
	public int[] childrenIDs;
	public ComparativeNode.ComparisonType comparisonType;
	public ComparativeNode.ComparisonOperator comparisonOperator;
	public string[] parameterName;
}


