<template>
  <q-page padding>
    <q-card>
      <q-card-section>
        <div class="text-h6">Réservations actuelles</div>
        <div v-if="room.value" class="text-subtitle1">
          Chambre sélectionnée : {{ room.value.Type }}
        </div>
        <div v-if="room.value && room.value.PricePerNight" class="text-subtitle2">
          Prix par nuit : {{ room.value.PricePerNight.Amount }} EUR
        </div>
        <div v-if="room.value && room.value.Infos" class="text-subtitle2">
          Informations :
          <ul>
            <li v-for="(info, index) in room.value.Infos" :key="index">
              {{ info }}
            </li>
          </ul>
        </div>
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
          <q-btn
            label="Supprimer mon compte"
            type="button"
            color="negative"
            @click="deleteUser"
          />
        </q-form>
      </q-card-section>
    </q-card>
    <q-card>
      <q-card-section>
        <div class="text-h6">
          <q-icon name="account_balance_wallet" class="q-mr-sm" />
          Gerer son portefeuille
        </div>
        <div class="text-subtitle2">
          Entrez le montant que vous souhaitez ajouter à votre portefeuille :
        </div>
        <q-input
          v-model="amount"
          type="number"
          placeholder="Montant"
          class="q-mt-md"
        />
        <q-select
          v-model="currency"
          :options="currencies"
          label="Devise"
          class="q-mt-md"
        />
        <q-btn
          label="Recharger"
          color="primary"
          class="q-mt-md"
          @click="rechargeWallet"
        />
        <div class="text-subtitle2 q-mt-md">
          Solde du portefeuille :
          {{ walletBalance.Balance ? walletBalance.Balance.Amount : 0 }} EUR
        </div>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import {
  getUserInfo,
  updateCustomer,
  deleteCustomer,
  creditWallet as apiRechargeWallet,
  getWallet as apiGetWalletBalance,
  createWallet as apiCreateWallet,
  debitWallet as apiDebitWallet,
} from "../../api/services/api";
import { Customer, Currency, Room } from "../../api/models/index";

const user = ref<Customer>({
  Id: "",
  FullName: "",
  Email: { Value: "" },
  PasswordHash: "",
  PhoneNumber: { Number: "" },
});

const currencies = ref(Object.values(Currency));

const amount = ref(0);
const currency = ref("EUR");
const walletBalance = ref({ Balance: { Amount: 0 } });
const room = ref<Room | null>({
  Id: undefined,
  Type: undefined,
  PricePerNight: {Amount: undefined, Currency: undefined},
  Infos: undefined
});

const rechargeWallet = async () => {
  if (!user.value.Id) {
    console.error("User ID is undefined");
    return;
  }
  console.log("Recharging wallet", user.value.Id, amount.value, currency.value);
  await apiRechargeWallet(user.value.Id, amount.value, currency.value);
  await getWalletBalance();
};

const getWalletBalance = async () => {
  if (!user.value.Id) {
    console.error("User ID is undefined");
    return;
  }
  console.log("Getting wallet balance", user.value.Id);
  try {
    walletBalance.value = await apiGetWalletBalance(user.value.Id);
  } catch (error: any) {
    console.error("Failed to get wallet balance", error);
    if (error.response && error.response.status === 404) {
      console.log("Wallet not found, creating a new one", user.value.Id);
      await apiCreateWallet(user.value.Id);
      walletBalance.value = await apiGetWalletBalance(user.value.Id);
    }
  }
};
const transformUserInfo = (userInfo: any): Customer => ({
  Id: userInfo.Id,
  FullName: userInfo.FullName,
  Email: { Value: userInfo.Email },
  PasswordHash: "",
  PhoneNumber: { Number: userInfo.PhoneNumber.Number },
});

const updateUserInfo = async () => {
  if (!user.value.Id) {
    console.error("User ID is undefined");
    return;
  }
  console.log("update Dzata", user.value.Id, user.value);
  await updateCustomer(user.value.Id, user.value);
};

const deleteUser = async () => {
  if (!user.value.Id) {
    console.error("User ID is undefined");
    return;
  }
  console.log("Deleting user", user.value.Id);
  await deleteCustomer(user.value.Id);
};

onMounted(async () => {
  const response = await getUserInfo();
  user.value = transformUserInfo(response);

  const roomString = localStorage.getItem("room");

  if (roomString) {
    room.value = JSON.parse(roomString);
  }

  if (user.value.Id) {
    await getWalletBalance();
    if (walletBalance.value.Balance.Amount === 0) {
      await apiCreateWallet(user.value.Id);
      await getWalletBalance();
    }
  } else {
    console.error("User ID is undefined");
  }
});
</script>
