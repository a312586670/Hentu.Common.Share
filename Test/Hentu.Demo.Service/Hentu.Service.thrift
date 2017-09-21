namespace csharp Hentu.Demo.Service.Thrift

/**
 * Demo枚举 用户性别
 */
enum EnumUserSex
{
	/**
	 * 男
	 */
	Man,
	/**
	 * 女
	 */
	Woman,
	/**
	 * 未知
	 */
	Unknown
}

/**
 * Demo类 用户
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
	 * Demo类 用户 添加
	 * @param user 用户
	 */
	bool DemoUser_Add(1:DemoUser user);
	/**
	 * Demo类 获取批量用户
	 */
	list<DemoUser> DemoUser_GetList();
}