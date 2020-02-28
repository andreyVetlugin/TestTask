<template>
  <div class="login-block shadow_medium">
    <h2>Вход</h2>
    <div>
      <LabeledInput v-model="login" placeholder="логин" required></LabeledInput>
      <LabeledInput
        v-model="password"
        placeholder="пароль"
        type="password"
        @keypress.enter="log_in"
        required
      ></LabeledInput>
    </div>
    <div>
      <CommonButton @click="log_in" text="Войти"></CommonButton>
      <h4 class="validation_message" v-text="error" v-show="error"></h4>
    </div>
  </div>
</template>

<style scoped>
h2 {
  margin: 16px 0;
}

.login-block {
  text-align: center;
  width: 640px;
  margin: auto;
  background-color: #3f3176;
  border: 1px solid #493c80;
  border-radius: 4px;
  padding: 64px 32px 80px;
  margin-top: 104px;
}

.labeled-input {
  margin-top: 16px;
  width: 368px;
}

.common-button {
  width: 224px;
  margin-top: 40px;
}

.validation_message {
  color: #f03000;
}
</style>

<script lang="ts">
import Vue from 'vue';
import LabeledInput from '../components/LabeledInput.vue';
import CommonButton from '../components/CommonButton.vue';
import { AxiosResponse, AxiosError } from 'axios';
import RequestSender from '@/requestSender';
export default Vue.extend({
  data() {
    return {
      login: '',
      password: '',
      error: '',
    };
  },
  computed: {
    sender(): RequestSender {
      return this.$store.getters.requestSender;
    },
  },
  methods: {
    log_in() {
      this.sender
        .auth(this.login, this.password)
        .then((resp: AxiosResponse) => {
          this.$router.push('/cards');
        })
        .catch((resp: AxiosError) => {
          this.error = resp && resp.response ? resp.response.data.error : '';
        });
    },
  },
  components: {
    LabeledInput,
    CommonButton,
  },
});
</script>
