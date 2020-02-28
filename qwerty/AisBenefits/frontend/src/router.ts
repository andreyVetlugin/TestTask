import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/cards',
      name: 'home',
      component: Home,
      props: (route) => ({
        cardId: route.query.cardId,
        term: route.query.term,
        tab: route.query.tab,
        page: parseInt(route.query.page, 10),
      }),
      meta: { requiresAuth: true },
    },
    {
      path: '/registry',
      name: 'registry',
      component: () => import('@/views/Registry.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/pension_update',
      name: 'GosPensionUpdate',
      component: () => import('@/views/GosPensionUpdate.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/configuration/roles',
      name: 'configuration',
      component: () => import('./views/RolesConfig.vue'),
      props: () => ({ tab: 'roles' }),
      meta: { requiresAuth: true },
    },
    {
      path: '/configuration/roles/edit',
      name: 'configuration_roles_edit',
      component: () => import('./views/RolesConfig.vue'),
      props: (route) => ({ tab: 'role_edit', roleId: route.query.id }),
      meta: { requiresAuth: true },
    },
    {
      path: '/configuration/roles/create',
      name: 'configuration_roles_create',
      component: () => import('./views/RolesConfig.vue'),
      props: () => ({ tab: 'role_edit' }),
      meta: { requiresAuth: true },
    },
    {
      path: '/configuration/dictionaries',
      name: 'configuration_dictionaries',
      component: () => import('./views/Config.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/configuration/egisso',
      name: 'configuration_egisso',
      component: () => import('./views/Egisso.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/create',
      name: 'create',
      component: () => import('./views/CardEdit.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/edit',
      name: 'edit',
      component: () => import('./views/CardEdit.vue'),
      props: (route) => ({ cardId: route.query.cardId, tab: route.query.tab }),
      meta: { requiresAuth: true },
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('./views/Login.vue'),
    },
    {
      path: '/logout',
      name: 'logout',
    },
    {
      path: '/archive/cards',
      name: 'archive_cards',
      props: (route) => ({
        cardId: route.query.cardId,
        term: route.query.term,
        tab: route.query.tab,
        page: parseInt(route.query.page, 10),
      }),
      component: () => import('./views/ArchiveCards.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/archive/registry',
      name: 'archive_registry',
      component: () => import('./views/ArchiveRegistry.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/mass_recount',
      name: 'mass_recount',
      component: () => import('./views/MassRecount.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/reports',
      name: 'reports',
      component: () => import('./views/Reports.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/zags',
      name: 'zags',
      component: () => import('./views/ZAGS.vue'),
      meta: { requiresAuth: true },
    },
  ],
});
