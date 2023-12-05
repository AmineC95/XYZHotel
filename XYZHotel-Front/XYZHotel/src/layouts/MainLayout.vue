<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        />
        <q-toolbar-title> XYZHotel </q-toolbar-title>
        <div>
          <q-btn label="Login" color="secondary" @click="goToLoginPage" />
        </div>
      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" show-if-above bordered>
      <q-list>
        <q-item-label header> Menu </q-item-label>

        <EssentialLink
          v-for="link in essentialLinks"
          :key="link.title"
          v-bind="link"
        />
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import EssentialLink, {
  EssentialLinkProps,
} from "components/EssentialLink.vue";

const essentialLinks: EssentialLinkProps[] = [
  {
    title: "Home",
    caption: "Home",
    icon: "home",
    link: "/",
  },
];

const router = useRouter();

const goToLoginPage = () => {
  router.push("/login");
};

const leftDrawerOpen = ref(false);

function toggleLeftDrawer() {
  leftDrawerOpen.value = !leftDrawerOpen.value;
}
</script>
