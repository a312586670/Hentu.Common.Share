namespace csharp Hentu.Demo.Service.Thrift

/**
 * Demoö�� �û��Ա�
 */
enum EnumUserSex
{
	/**
	 * ��
	 */
	Man,
	/**
	 * Ů
	 */
	Woman,
	/**
	 * δ֪
	 */
	Unknown
}

/**
 * Demo�� �û�
 */
struct DemoUser
{
	1:i32 UserId;
	2:i64 UserIds;
	3:string UserName;
	4:bool IsValid;
	5:list<string> UserEmails;
	6:map<string, string> UserAges;
	7:EnumUserSex UserSex;
	8:i64 RegTime;
}


service HentuDemoService
{ 
	/**
	 * Demo�� �û� ���
	 * @param user �û�
	 */
	bool DemoUser_Add(1:DemoUser user);
	/**
	 * Demo�� ��ȡ�����û�
	 */
	list<DemoUser> DemoUser_GetList();
}