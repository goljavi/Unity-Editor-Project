using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ComparativeNode : BaseNode, INeedsChildren, ISetableValues {

	private BaseNode[] children = new BaseNode[2]; //Los nodos hijos a los que se va a elegir
	private int[] childrenIDs = new int[2]; //IDs de los hijos para futura asignacion

	//Tipo de comparacion activa
	public enum ComparisonType { Float, Int, Bool }
	private ComparisonType activeType;

	//Operacion comparativa activa
	public enum ComparisonOperator { Equals, NotEqual, Lesser, LesserEquals, Greater, GreaterEquals }
	public ComparisonOperator activeOperator;


	//Valores con los que se van a comparar
	private float[] comparedFloats = new float[2];
	private int[] comparedInts = new int[2];
	private bool comparedBool;

	public override string GetNodeType { get { return "Comparison"; } }


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

	#region ISetableValue asignacion de valores externos
	public void SetFloat(int id, float value) {
		if (ValidIndex(id))
		{
			comparedFloats[id] = value;
		}
	}

	public void SetInt(int id, int value) {
		if (ValidIndex(id))
		{
			comparedInts[id] = value;
		}
	}

	public void SetBool(int id, bool value) {
		comparedBool = value;
	}
	#endregion ISetableValue asignacion de valores externos

	//Ejecuta la comparacion correspondiente
	public bool Compare() {
		switch (activeType)
		{
			case ComparisonType.Float:
				return CompareFloat();
			case ComparisonType.Int:
				return CompareInt();
			case ComparisonType.Bool:
				return comparedBool;
			default:
				Debug.LogWarning("Invalid Comparison Type Enum at " + this.ToString());
				return false;
		}
	}

	//Comparacion de floats
	private bool CompareFloat() {
		switch (activeOperator)
		{
			case ComparisonOperator.Equals:
				return comparedFloats[0] == comparedFloats[1];
			case ComparisonOperator.NotEqual:
				return !(comparedFloats[0] == comparedFloats[1]);
			case ComparisonOperator.Lesser:
				return comparedFloats[0] < comparedFloats[1];
			case ComparisonOperator.LesserEquals:
				return comparedFloats[0] <= comparedFloats[1];
			case ComparisonOperator.Greater:
				return comparedFloats[0] > comparedFloats[1];
			case ComparisonOperator.GreaterEquals:
				return comparedFloats[0] >= comparedFloats[1];
			default:
				Debug.LogWarning("Invalid Comparison Operator Enum at " + this.ToString());
				return false;
		}
	}

	//comparacion de ints
	private bool CompareInt() {
		switch (activeOperator)
		{
			case ComparisonOperator.Equals:
				return comparedInts[0] == comparedInts[1];
			case ComparisonOperator.NotEqual:
				return !(comparedInts[0] == comparedInts[1]);
			case ComparisonOperator.Lesser:
				return comparedInts[0] < comparedInts[1];
			case ComparisonOperator.LesserEquals:
				return comparedInts[0] <= comparedInts[1];
			case ComparisonOperator.Greater:
				return comparedInts[0] > comparedInts[1];
			case ComparisonOperator.GreaterEquals:
				return comparedInts[0] >= comparedInts[1];
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
		comparedFloats = converted.floatValues;
		comparedInts = converted.intValues;
		comparedBool = converted.boolValue;
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
			floatValues = new float[2] { comparedFloats[0], comparedFloats[1] },
			intValues = new int[] { comparedInts[0], comparedInts[1] },
			boolValue = comparedBool
		});
	}

}

//Datos del nodo.
[System.Serializable]
public struct ComparativeNodeData {
	public int[] childrenIDs;
	public ComparativeNode.ComparisonType comparisonType;
	public ComparativeNode.ComparisonOperator comparisonOperator;
	public float[] floatValues;
	public int[] intValues;
	public bool boolValue;
}


