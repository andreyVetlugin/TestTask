<template>
  <div v-if="isAuthorized" class="left-menu" :class="{compressed: compressed}">
    <div class="user-icon-container">
      <div class="user-icon">
        <i class="fa fa-user-o"></i>
      </div>
      <div class="toggle-button">
        <i class="fa fa-arrow-right" @click="toggleMenu" v-show="compressed"></i>
      </div>
    </div>
    <div class="left-menu-hidden-block">
      <h5 class="user-name">{{name}}</h5>
      <div class="toggle-button">
        <i class="fa fa-arrow-left" @click="toggleMenu" v-show="!compressed"></i>
      </div>
      <div class="navigation-bar" @click="$store.commit('setSelectedCardId', '')">
        <router-link class="uppercase" to="/cards">
          База данных
          <span class="counter" v-if="counters.cards">{{counters.cards}}</span>
        </router-link>
        <router-link class="uppercase" to="/registry">Реестр</router-link>
        <Expandable class="link_fetch" v-model="mejvedExpanded">
          <template slot="title">
            <h5 class="uppercase pointer-cursor">Межвед</h5>
          </template>
          <template slot="content">
            <router-link class="uppercase" to="/pension_update">
              ПФР
              <span class="counter always" v-if="counters.pfr">{{counters.pfr}}</span>
            </router-link>
            <router-link class="uppercase" to="/zags">ЗАГС</router-link>
          </template>
        </Expandable>
        <router-link class="uppercase" to="/mass_recount">Массовые перерасчёты</router-link>
        <router-link class="uppercase" to="/reports">Сводный отчёт</router-link>
        <Expandable class="link_fetch" v-model="archiveExpanded">
          <template slot="title">
            <h5 class="uppercase pointer-cursor">Архив</h5>
          </template>
          <template slot="content">
            <router-link class="smaller-text" to="/archive/cards">
              База данных
              <span
                class="counter"
                v-if="counters.archiveCards"
              >{{counters.archiveCards}}</span>
            </router-link>
            <router-link class="smaller-text" to="/archive/registry">Реестр</router-link>
          </template>
        </Expandable>
        <Expandable class="link_fetch" v-model="adminExpanded">
          <template slot="title">
            <h5 class="uppercase pointer-cursor">Администрирование</h5>
          </template>
          <template slot="content">
            <router-link class="smaller-text" to="/configuration/roles">Доступ и роли</router-link>
            <router-link class="smaller-text" to="/configuration/dictionaries">Справочники</router-link>
            <router-link class="smaller-text" to="/configuration/egisso">ЕГИССО</router-link>
          </template>
        </Expandable>
        <router-link class="uppercase" to="/logout">Выйти</router-link>
      </div>
    </div>
  </div>
</template>

<style>
.link_fetch h5 {
  padding: 8px 0;
  margin: 0;
}
.link_fetch.expanded h5 {
  color: #ffffff;
}
.uppercase {
  text-transform: uppercase;
}
.smaller-text {
  font-size: 90%;
}

.counter {
  opacity: 0;
  float: right;
  margin: 0 -30px 0 0;
  padding: 0;
  transition: opacity 0.3s ease, margin 0.3s ease-in-out;
}
.counter.always,
.navigation-bar a:hover .counter,
.router-link-active .counter {
  opacity: 1;
  margin: 0;
}

.left-menu {
  flex: 0 0 312px;
  width: 312px;
  height: 100%;
}

.left-menu .expandable-content a {
  margin-left: 0.5em;
}

.toggle-button * {
  display: none;
}
.toggle-button .fa {
  height: 16px;
  font-size: 16px;
}
.toggle-button .fa-arrow-right {
  border-right: 1px solid #d49e35;
}
.toggle-button .fa-arrow-left {
  border-left: 1px solid #d49e35;
}

.user-icon-container {
  display: inline-block;
  width: 64px;
  height: 100%;
  text-align: center;
  position: fixed;
  z-index: 49;
  background-color: #503f96;
}
.user-icon {
  display: inline-block;
  border: 2px solid #2c205e;
  border-radius: 50%;
  margin: 24px 16px;
  padding-top: 6px;
  width: 32px;
  height: 32px;
  overflow: hidden;
  box-sizing: border-box;
  color: #2c205e;
  font-size: 26px;
  line-height: 0;
}
.user-icon .fa-user-o {
  font-weight: 500;
}

.left-menu-hidden-block {
  position: fixed;
  background-color: #503f96;
  padding: 12px 32px 0 0;
  flex: 0 0 248px;
  width: 248px;
  height: 100%;
  box-sizing: border-box;
  left: 64px;
  z-index: 48;
  box-shadow: #1e1542 0 16px 16px 0;
}

.navigation-bar {
  margin: 24px 0px 80px 0px;
  padding: 16px 0px;
  display: flex;
  flex-direction: column;
  border-top: 1px solid #35296b;
}

.navigation-bar a {
  flex: 0 0 32px;
  padding: 8px 0;
  color: #c6bcee;
  box-sizing: border-box;
  text-decoration: none;
  cursor: pointer;
  font-family: "Rubik-Light";
  line-height: 18pt;
  width: 100%;
  display: inline-block;
}

.navigation-bar a.router-link-active,
.navigation-bar a.router-link-exact-active {
  color: #f1b238;
}

.navigation-bar .pointer-cursor:hover,
.navigation-bar a:hover {
  color: #d49e35;
}

.navigation-bar a:active {
  color: #b7882e;
}

@media (max-width: 1400px) {
  .toggle-button * {
    display: inline-block;
  }
  .left-menu {
    transition: width 0.5s ease-in-out, flex 0.5s ease-in-out;
  }
  .left-menu-hidden-block {
    transition: left 0.5s ease-in-out;
  }
  .left-menu {
    flex: 0 0 64px;
    width: 64px;
  }
  .toggle-button {
    color: #d49e35;
    height: 24px;
  }
  .compressed .user-icon {
    border-color: #d49e35;
    color: #d49e35;
  }
  .compressed .left-menu-hidden-block {
    left: -184px;
  }
}
</style>

<script lang="ts">
import Vue from 'vue';
import Expandable from './Expandable.vue';
import { Route } from 'vue-router';
export default Vue.extend({
  data: () => {
    return {
      compressed: true,
      archiveExpanded: false,
      adminExpanded: false,
      mejvedExpanded: false,
      counters: {
        cards: 0,
        pfr: 0,
        archiveCards: 0,
      },
    };
  },
  mounted() {
    this.$store.getters.requestSender.getMenuCounters()
      .then((resp: any) => {
        if (resp.data) {
          this.counters.cards = resp.data.cards || 0;
          this.counters.pfr = resp.data.pfr || 0;
          this.counters.archiveCards = resp.data.archiveCards || 0;
        }
      });
  },
  methods: {
    toggleMenu() {
      this.compressed = !this.compressed;
    },
  },
  computed: {
    name(): string {
      return this.$store.getters.userName;
    },
    permitions(): Array<{}> {
      return this.$store.getters.permissionsList;
    },
    isAuthorized(): boolean {
      return this.$store.getters.isAuthorized;
    },
  },
  watch: {
    $route(val: Route) {
      this.compressed = true;
      this.archiveExpanded = val.path.includes('archive');
      this.adminExpanded = val.path.includes('configuration');
      this.mejvedExpanded = val.path.includes('zags') || val.path.includes('pension_update');
    },
    archiveExpanded(val: boolean) {
      if (val) {
        this.adminExpanded = false;
        this.mejvedExpanded = false;
      }
    },
    adminExpanded(val: boolean) {
      if (val) {
        this.archiveExpanded = false;
        this.mejvedExpanded = false;
      }
    },
    mejvedExpanded(val: boolean) {
      if (val) {
        this.archiveExpanded = false;
        this.adminExpanded = false;
      }
    },
  },
  components: {
    Expandable,
  },
});
</script>
