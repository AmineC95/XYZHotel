<template>
  <q-page padding>
    <q-card>
      <q-card-section>
        <div class="text-h6">Réservations actuelles</div>
      </q-card-section>
    </q-card>
    <q-card>
      <q-card-section>
        <div class="text-h6">Modifier les informations de l'utilisateur</div>
        <q-form @submit="updateUserInfo">
          <q-input v-model="user.FullName" label="Nom complet" />
          <q-input v-model="user.PhoneNumber.Number" label="Telephone" />
          <q-input v-model="user.Email.Value" label="Email" />
          <q-input v-model="user.PasswordHash" label="Mot de passe" />
          <q-btn label="Mettre à jour" type="submit" color="primary" />
        </q-form>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { getUserInfo, updateCustomer } from "../../api/services/api";
import { Customer } from "../../api/models/index";

const user = ref<Customer>({
  Id: "",
  FullName: "",
  Email: { Value: "" },
  PasswordHash: "",
  PhoneNumber: { Number: "" },
});

const transformUserInfo = (userInfo: any): Customer => ({
  Id: userInfo.Id,
  FullName: userInfo.FullName,
  Email: { Value: userInfo.Email },
  PasswordHash: "",
  PhoneNumber: { Number: userInfo.PhoneNumber.Number },
});

const updateUserInfo = async () => {
  if (!user.value.Id) {
    console.error('User ID is undefined');
    return;
  }
  console.log("update Dzata",user.value.Id, user.value);
  await updateCustomer(user.value.Id, user.value);
};

onMounted(async () => {
  const response = await getUserInfo();
  user.value = transformUserInfo(response);
});
</script>
