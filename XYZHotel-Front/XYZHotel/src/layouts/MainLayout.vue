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
          <q-btn v-if="isLoggedIn" label="Deconnexion" color="secondary" @click="logout" />
          <q-btn v-else label="Connexion" color="secondary" @click="goToLoginPage" />
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
      <q-list v-if="isLoggedIn">
        <q-item-label header>  Mon espace </q-item-label>

        <EssentialLink
          v-for="link in customerLinks"
          :key="link.title"
          v-bind="link"
        />
      </q-list>
    </q-drawer>

    <q-page-container class="background-color" >
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, ref } from "vue";
import { useRouter } from "vue-router";
import EssentialLink, {
  EssentialLinkProps,
} from "components/EssentialLink.vue";
import { getUserInfo } from "../api/services/api";

const isLoggedIn = ref(localStorage.getItem('token') !== null);

const essentialLinks: EssentialLinkProps[] = [
  {
    title: "Home",
    caption: "Home",
    icon: "home",
    link: "/",
  },
  {
    title: "Réservation",
    caption: "Reservation",
    icon: "check",
    link: "#/booking",
  },
];

const customerLinks: EssentialLinkProps[] = [
  {
    title: "Espace client",
    caption: "Gestion de compte",
    icon: "account_circle",
    link: "#/dashboard",
  },
  {
    title: "Mes Reservation",
    caption: "Mes Reservation",
    icon: "",
    link: "#/booking",
  },
];

const userInfo = ref(null);
const router = useRouter();

const goToLoginPage = () => {
  router.push("/login");
};

const updateLoginStatus = () => {
  isLoggedIn.value = localStorage.getItem('token') !== null;
};

const logout = () => {
  localStorage.removeItem('token');
  isLoggedIn.value = false;
  router.push("/");
};

const leftDrawerOpen = ref(false);

function toggleLeftDrawer() {
  leftDrawerOpen.value = !leftDrawerOpen.value;
}

onMounted(async () => {
  window.addEventListener('storage', updateLoginStatus);
});

onUnmounted(() => {
  window.removeEventListener('storage', updateLoginStatus);
});

</script>

<style>
.background-color{
  background-color: #f3f7fb;
}
</style>
