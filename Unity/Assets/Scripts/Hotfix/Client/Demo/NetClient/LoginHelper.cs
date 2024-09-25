namespace ET.Client
{
    public static class LoginHelper
    {
        public static async ETTask Login(Scene root, string account, string password)
        {
            root.RemoveComponent<ClientSenderComponent>();

            ClientSenderComponent clientSenderComponent = root.AddComponent<ClientSenderComponent>();

            NetClient2Main_Login response = await clientSenderComponent.LoginAsync(account, password);
            if (response.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"�����¼ʧ�ܣ� Error : {response.Error}");
                return;
            }

            Log.Debug("�����¼�ɹ�������");

            string Token = response.Token;

            // ��ȡ�������б�
            C2R_GetServerInfos c2RGetServerInfos = C2R_GetServerInfos.Create();
            c2RGetServerInfos.Account = account;
            c2RGetServerInfos.Token = response.Token;
            R2C_GetServerInfos r2CGetServerInfos = await clientSenderComponent.Call(c2RGetServerInfos) as R2C_GetServerInfos;
            if (r2CGetServerInfos.Error != ErrorCode.ERR_Success)
            {
                Log.Error("����������б�ʧ�ܣ�");
                return;
            }

            ServerInfoProto serverInfoProto = r2CGetServerInfos.ServerInfosList[0];
            Log.Debug($"����������б�ɹ�, ��������: {serverInfoProto.ServerName} ����ID: {serverInfoProto.Id}");

            // ��ȡ������ɫ�б�
            C2R_GetRoles c2RGetRoles = C2R_GetRoles.Create();
            c2RGetRoles.Token = Token;
            c2RGetRoles.Account = account;
            c2RGetRoles.ServerId = serverInfoProto.Id;
            R2C_GetRoles r2CGetRoles = await clientSenderComponent.Call(c2RGetRoles) as R2C_GetRoles;
            if (r2CGetRoles.Error != ErrorCode.ERR_Success)
            {
                Log.Error("����������ɫ�б�ʧ�ܣ�");
                return;
            }

            RoleInfoProto roleInfoProto = default;
            if (r2CGetRoles.RoleInfo.Count <= 0)
            {
                C2R_CreateRole c2RCreateRole = C2R_CreateRole.Create();
                c2RCreateRole.Token = Token;
                c2RCreateRole.Account = account;
                c2RCreateRole.ServerId = serverInfoProto.Id;
                c2RCreateRole.Name = account;
                
                R2C_CreateRole r2CCreateRole = await clientSenderComponent.Call(c2RCreateRole) as R2C_CreateRole;
                if (r2CCreateRole.Error != ErrorCode.ERR_Success)
                {
                    Log.Error("����������ɫʧ�ܣ�");
                    return;
                }

                roleInfoProto = r2CCreateRole.RoleInfo;
            }
            else
            {
                roleInfoProto = r2CGetRoles.RoleInfo[0];
            }
            
            // �����ȡRealmKey
            C2R_GetRealmKey c2RGetRealmKey = C2R_GetRealmKey.Create();
            c2RGetRealmKey.Token = Token;
            c2RGetRealmKey.Account = account;
            c2RGetRealmKey.ServerId = serverInfoProto.Id;
            R2C_GetRealmKey r2CGetRealmKey = await clientSenderComponent.Call(c2RGetRealmKey) as R2C_GetRealmKey;

            if (r2CGetRealmKey.Error != ErrorCode.ERR_Success)
            {
                Log.Error("��ȡRealmKeyʧ�ܣ�");
                return;
            }
            
            // ������Ϸ��ɫ����Map��ͼ
            NetClient2Main_LoginGame netClient2MainLoginGame =
                    await clientSenderComponent.LoginGameAsync(account, r2CGetRealmKey.Key, roleInfoProto.Id, r2CGetRealmKey.Address);
            if (netClient2MainLoginGame.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"������Ϸʧ�ܣ�{netClient2MainLoginGame.Error}");
                return;
            }
            
            Log.Debug("������Ϸ�ɹ�������");

            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
}