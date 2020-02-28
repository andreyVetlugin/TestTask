<template>
  <div class="page_layout roles_config">
    <div class="dictionaries-list">
      <div
        :class="{ selected: tab === 'roles' || tab === 'role_edit'}"
        @click="tab = 'roles'"
        class="dictionary shadow_light"
      >
        <h2>Роли</h2>
      </div>
      <div
        :class="{ selected: tab === 'users'}"
        @click="tab = 'users'"
        class="dictionary shadow_light"
      >
        <h2>Пользователи</h2>
      </div>
    </div>
    <div class="roles_config-view shadow_medium">
      <div v-show="tab === 'roles'" class="roles_config_tab-content">
        <h1>Роли</h1>
        <div class="row">
          <h5 style="flex: 0 0 64px;"></h5>
          <h5>Роль</h5>
          <h5>Права</h5>
        </div>
        <Scrollable :class="'roles-container'">
          <div class="row" v-for="role in rolesList" :key="role.id">
            <EditButton @click="openCreateEditRole(role.id)"></EditButton>
            <h4>{{role.name}}</h4>
            <h4>
              <h4
                v-for="(perm, i) in role.permissions"
                :key="role.id + '-' + i"
              >{{allPermissions && allPermissions.length ? (allPermissions.find((el) => el.id === perm) || '').title : ''}}</h4>
            </h4>
          </div>
        </Scrollable>
      </div>
      <div v-show="tab === 'users'" class="roles_config_tab-content">
        <h1>Пользователи</h1>
        <div class="row">
          <h5 style="flex: 0 0 64px;"></h5>
          <h5>ФИО</h5>
          <h5>Роли</h5>
        </div>
        <Scrollable :class="'roles-container'">
          <div class="row" v-for="user in usersList" :key="user.id">
            <EditButton @click="openCreateEditUser(user.id)"></EditButton>
            <h4>{{user.secondName}} {{user.name}}</h4>
            <h4>
              <h4 v-for="role in user.roles" :key="role.id">{{role.name || ''}}</h4>
            </h4>
          </div>
        </Scrollable>
      </div>
      <div v-show="tab === 'role_edit'" class="roles_config_tab-content role-edit">
        <h1 v-text="roleId ? 'Редактировать роль' : 'Добавить роль'"></h1>
        <LabeledInput v-model="newRole" placeholder="Название"></LabeledInput>
        <div
          class="row edit-row"
          v-for="(perm, i) in newRolePermissions"
          :key="i"
          @click="perm.selected = !perm.selected"
          :class="{ selected: perm.selected }"
        >
          <i class="fa fa-check"></i>
          <h4>{{perm.title}}</h4>
        </div>
        <CommonButton text="Сохранить" @click="createEditRole"></CommonButton>
      </div>
      <div v-show="tab === 'user_edit'" class="roles_config_tab-content user_edit">
        <h1 v-text="userId ? 'Редактировать пользователя' : 'Добавить пользователя'"></h1>
        <LabeledInput v-model="newUser.secondName" placeholder="Фамилия"></LabeledInput>
        <LabeledInput v-model="newUser.name" placeholder="Имя"></LabeledInput>
        <LabeledInput v-model="newUser.login" placeholder="Логин"></LabeledInput>
        <LabeledInput v-model="newUser.password" type="password" placeholder="Пароль"></LabeledInput>
        <div>
          <DropDown placeholder="выберите роль" @input="addRole" :list="rolesDdownList"></DropDown>
        </div>
        <div
          class="row edit-row"
          v-for="role in newUser.roles"
          :key="role.id"
          @click="role.selected = !role.selected"
          :class="{ selected: role.selected }"
        >
          <CommonButton
            noBackground
            class="delete-button"
            html="<i class='fa fa-times'></i>"
            @click="removeRole(role.id)"
          ></CommonButton>
          <h4>{{role.name}}</h4>
        </div>
        <CommonButton text="Сохранить" @click="createEditUser"></CommonButton>
      </div>
    </div>
    <div class="command_menu">
      <PlusButton v-show="tab === 'roles'" @click="openCreateEditRole('')"></PlusButton>
      <PlusButton v-show="tab === 'users'" @click="openCreateEditUser('')"></PlusButton>
      <CrossButton v-show="tab === 'role_edit'" @click="closeCreateEditRole"></CrossButton>
      <CrossButton v-show="tab === 'user_edit'" @click="closeCreateEditUser"></CrossButton>
    </div>
  </div>
</template>

<style>
.dictionaries-list {
  display: flex;
  flex-wrap: nowrap;
  align-content: flex-start;
  align-items: baseline;
  overflow-x: auto;
  overflow-y: none;
  flex: 0 0;
  flex-basis: calc(33% - 32px);
  flex-direction: column;
  justify-content: flex-start;
}
.dictionary {
  background-color: #3f3176;
  border: 1px solid #493c80;
  border-radius: 4px;
  box-sizing: border-box;
  padding: 16px 24px;
  font-size: 0;
  margin: 0 16px 8px;
  width: calc(100% - 32px);
  flex: 0 0 auto;
}
.dictionary.selected {
  background-color: #503f96;
  border: 1px solid #3a3066;
}
.dictionary h2 {
  margin: 0;
}

.roles_config-view {
  display: flex;
  height: calc(100%);
  border-radius: 4px;
  background-color: #6254a3;
  box-sizing: border-box;
  flex-wrap: nowrap;
  flex: 0 0;
  /* flex-basis: calc(100% - 112px); */
  /* margin-left: 16px; */
  flex-basis: calc(67% - 64px);
}

.roles_config .roles_config_tab-content {
  padding: 0 48px;
  box-sizing: border-box;
  display: flex;
  flex: 0 0 100%;
  width: 100%;
  height: 100%;
  align-content: flex-start;
  align-items: baseline;
  flex-wrap: nowrap;
  flex-direction: column;
}
.roles_config .scrollable-container {
  width: calc(100% + 34px);
  height: 100%;
  margin-bottom: 48px;
  margin-right: -24px;
}
.roles_config h1 {
  margin: 24px 0;
  flex: 0 0;
  width: 100%;
}
.roles_config h4,
.roles_config h5 {
  margin: 0;
  flex: 0 0 33%;
}
.roles_config_tab-content.role-edit .text-input {
  background-color: #503f96;
  margin-top: 24px;
  width: 240px;
  flex: 0 0 64px;
}
.roles_config_tab-content.role-edit .common-button {
  margin-top: 24px;
  width: 240px;
  flex: 0 0 64px;
}
.roles_config_tab-content.user_edit .text-input {
  background-color: #503f96;
  margin-top: 24px;
  width: 240px;
  flex: 0 0 64px;
}
.roles_config_tab-content.user_edit > .common-button {
  margin-top: 24px;
  width: 240px;
  flex: 0 0 64px;
}
.roles_config_tab-content.user_edit .dropdown {
  margin-top: 24px;
}
.roles_config_tab-content.user_edit .dropdown-input .text-input {
  background-color: transparent;
  margin: 0;
  flex-basis: calc(100% - 64px);
}
.roles_config_tab-content.user_edit .dropdown-input .common-button {
  margin: 0;
  width: 64px;
}
.roles_config_tab-content.user_edit .edit-row {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  padding: 0;
}
.roles_config_tab-content.role-edit .fa {
  color: transparent;
  font-size: 10px;
  padding: 4px;
  margin: 2px 8px;
  height: 18px;
  width: 18px;
  border-radius: 50%;
  background-color: #503f96;
  box-sizing: border-box;
}
.roles_config_tab-content.role-edit .row:hover .fa {
  background-color: #8a7ec0;
}

.roles_config_tab-content.role-edit .row:active .fa {
  background-color: #6253a2;
  color: #ffffff;
}
.roles_config_tab-content.role-edit .row.selected .fa {
  color: #ffffff;
}

.roles_config .row {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  flex: 0 0 auto;
  width: 100%;
  border-bottom: 1px solid #534590;
  padding: 24px 0;
}
.roles_config .row.header-row {
  border-top: 1px solid #534590;
  margin-top: 24px;
}
.roles_config .row.edit-row {
  flex-wrap: nowrap;
  justify-content: flex-start;
}
.roles_config .row.edit-row h4 {
  flex: 1 1 auto;
}
.roles_config .row:last-child {
  border-bottom: none;
}
.roles_config .new-row {
  margin: 24px 0;
  flex: 0 0 64px;
}
.roles-container {
  width: 100%;
  height: min-content;
  max-height: calc(100% - 64px);
}
</style>

<script lang="ts">
import Vue from 'vue';
import CommonButton from '@/components/CommonButton.vue';
import CrossButton from '@/components/Common/Buttons/CrossButton.vue';
import PlusButton from '@/components/Common/Buttons/PlusButton.vue';
import EditButton from '@/components/Common/Buttons/EditButton.vue';
import Scrollable from '@/components/Scrollable.vue';
import DropDown from '@/components/DropDown.vue';
import LabeledInput from '@/components/LabeledInput.vue';
import RequestSender from '@/requestSender';
import User from '@/models/users/user';
export default Vue.extend({
  props: {
  },
  data() {
    return {
      tab: 'roles',
      roleId: '',
      userId: '',
      newUser: new User({}),
      newRole: '',
      newRolePermissions: [{}],
      selectedRole: { name: '', permissions: [''] },
    };
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    rolesList(): Array<{ id: string, name: string, permissions: string[] }> {
      return this.$store.getters.rolesList;
    },
    rolesDdownList(): Array<{ value: string, title: string }> {
      return this.rolesList.map((el) => {
        return {
          value: el.id,
          title: el.name,
        };
      });
    },
    usersList(): User[] {
      return this.$store.getters.usersList;
    },
    allPermissions(): Array<{}> {
      return this.$store.getters.permissionsList;
    },
  },
  mounted() {
    this.requestSender.getAllRoles();
    this.requestSender.getAllPermissions();
    this.requestSender.getAllUsers();
  },
  watch: {
    allPermissions() {
      this.selectedRole = this.roleId
        ? (this.rolesList.find((el: any) =>
          el.id === this.roleId,
        ) || { name: '', permissions: [''] })
        : { name: '', permissions: [''] };
      this.newRole = (this.rolesList.find((el: any) => el.id === this.roleId) || { name: '' }).name;
      this.newRolePermissions = this.allPermissions.map((el: any) => {
        return {
          id: el.id,
          title: el.title,
          selected: (Boolean)(
            this.selectedRole.permissions &&
            this.selectedRole.permissions.find((elem) => elem === el.id) !== undefined,
          ),
        };
      });
    },
    roleId(val) {
      this.selectedRole = val
        ? (this.rolesList.find((el: any) =>
          el.id === val,
        ) || { name: '', permissions: [''] })
        : { name: '', permissions: [''] };
      this.newRole = (this.rolesList.find((el: any) => el.id === this.roleId) || { name: '' }).name;
      this.newRolePermissions = this.allPermissions.map((el: any) => {
        return {
          id: el.id,
          title: el.title,
          selected: (Boolean)(
            this.selectedRole.permissions &&
            this.selectedRole.permissions.find((elem) => elem === el.id) !== undefined,
          ),
        };
      });
    },
    userId(id) {
      this.requestSender.getUser(id).then((user) => this.newUser = user);
    },
  },
  methods: {
    openCreateEditRole(id?: string) {
      this.$data.roleId = id;
      this.$data.tab = 'role_edit';
    },
    closeCreateEditRole() {
      this.$data.roleId = '';
      this.$data.tab = 'roles';
    },
    openCreateEditUser(id?: string) {
      this.$data.userId = id;
      if (!id) {
        this.newUser = new User({});
      }
      this.$data.tab = 'user_edit';
    },
    closeCreateEditUser() {
      this.$data.userId = '';
      this.$data.tab = 'users';
    },
    createEditRole() {
      this.requestSender.createEditRole({
        id: this.roleId,
        name: this.newRole,
        permissions: this.newRolePermissions.filter((el: any) => el.selected).map((el: any) => el.id),
      }).then(() => {
        this.$data.tab = 'roles';
      });
    },
    createEditUser() {
      this.requestSender.createEditUser(this.newUser)
        .then(() => {
          this.$data.tab = 'users';
        });
    },
    addRole(id: string) {
      const role = this.rolesList.find((r) => r.id === id);
      if (role && this.newUser.roles.every((r) => r.id !== id)) {
        this.newUser.roles.push(role);
      }
    },
    removeRole(id: string) {
      if (id) {
        this.newUser.roles = this.newUser.roles.filter((r) => r.id !== id);
      }
    },
  },
  components: {
    CommonButton,
    CrossButton,
    PlusButton,
    EditButton,
    Scrollable,
    LabeledInput,
    DropDown,
  },
});
</script>
