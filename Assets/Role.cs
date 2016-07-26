using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NCMB;
using System.Linq;
using System.Collections.Generic;
public class Role : MonoBehaviour {
	public InputField RoleName;
	public Text RoleText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddRole(){
		NCMBUser currentUser;
		currentUser = NCMBUser.CurrentUser;
		//InputFiealdに記入されたRoleNameをもとにロールクラスを検索
		NCMBRole.GetQuery ().WhereEqualTo ("roleName", RoleName.text).FindAsync ((roleList, error) => {
			NCMBRole role = roleList.FirstOrDefault ();
			if (role != null) {
				//既存のロールと一致したら、ユーザーを既存ロールに追加する
				role.Users.Add (currentUser);
				role.SaveAsync ((NCMBException addRoleError) => {
					if(addRoleError != null){
						UnityEngine.Debug.Log ("role追加に失敗: " + addRoleError.ErrorMessage);
					}else{
						UnityEngine.Debug.Log ("roleへの追加に成功");
					}
				});

			}else{
				//既存ロールと一致しなければ、ロールを新たに作成しそのロールに追加する
				NCMBRole newRole = new NCMBRole (RoleName.text);
				newRole.Users.Add (currentUser);
				newRole.SaveAsync ((NCMBException newRoleError) => {
					if(newRoleError != null){
						UnityEngine.Debug.Log ("role新規登録に失敗: " + newRoleError.ErrorMessage);
					}else{
						UnityEngine.Debug.Log ("role新規登録に成功");
					}
				});
			}
		});
	}

	//現在ログイン中のユーザーに関連付いているロール情報を取得する
	public void seeBelongRole(){
		NCMBUser currentUser;
		currentUser = NCMBUser.CurrentUser;
		NCMBQuery<NCMBRole> query = NCMBRole.GetQuery ();
		query.WhereEqualTo ("belongUser.objectId", currentUser.ObjectId);
		query.FindAsync ((List<NCMBRole> roleList,NCMBException e) => {
			if (e != null) {
				//エラー処理
			} else {
				foreach (NCMBRole role in roleList) {
					RoleText.text += role.Name;
					RoleText.text += "\n";
					Debug.Log( role.Name );
					Debug.Log ("objectId : " + role.ObjectId);
					Debug.Log ("roleName:" + role.Name);
				}
			}
		});
	}

}

