import Role from './role';

export default class User {

    public id: string;
    public login: string;
    public password: string;
    public name: string;
    public secondName: string;
    public roles: Role[];
    public permissions: string[];

    constructor(data: any) {
        this.id = data.id;
        this.login = data.login || '';
        this.password = '';
        this.name = data.name || '';
        this.secondName = data.secondName || '';
        this.roles = (data.roles || []).map((el: any) => new Role(el)) || [];
        this.permissions = this.roles.map(
            (elem: { permissions: string[] }) => {
                return elem.permissions;
            }).join().split(',');
    }

    public toServerModel() {
        return {
            id: this.id,
            login: this.login,
            password: this.password,
            name: this.name,
            secondName: this.secondName,
            roleIds: this.roles.map((role) => role.id) || [],
        };
    }
}
